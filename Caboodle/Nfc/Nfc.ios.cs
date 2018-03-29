using System;
using System.IO;
using CoreFoundation;
using CoreNFC;
using Foundation;

namespace Microsoft.Caboodle
{
    public static partial class Nfc
    {
        static NfcReader nfcReader;

        internal static bool IsSupported
            => NFCNdefReaderSession.ReadingAvailable;

        static void EnsureReader()
        {
        }

        static void PlatformPublishMessage(string messageType, byte[] message)
        {
            throw new NotSupportedException();
        }

        static void PlatformStopPublishing()
        {
            throw new NotSupportedException();
        }

        static void StartTagListeners()
        {
            EnsureReader();
            nfcReader.Start();
        }

        static void StopTagListeners()
        {
            nfcReader.Stop();
        }

        static void StartNdefMessageListeners()
        {
            EnsureReader();
            nfcReader.Start();
        }

        static void StopNdefMessageListeners()
        {
            nfcReader.Stop();
        }

        class NfcReader : NSObject, INFCNdefReaderSessionDelegate
        {
            NFCNdefReaderSession session;

            public void Start()
            {
                if (session != null)
                {
                    session = new NFCNdefReaderSession(this, DispatchQueue.CurrentQueue, false);
                    session.BeginSession();
                }
            }

            public void Stop()
            {
                session?.InvalidateSession();
                session = null;
            }

            public void DidDetect(NFCNdefReaderSession session, NFCNdefMessage[] messages)
            {
                TagArrived?.Invoke(NfcTagEventArgs.Empty);

                foreach (var message in messages)
                {
                    NdefMessageReceived?.Invoke(new NdefMessageReceivedEventArgs(message.ToByteArray()));
                }
            }

            public void DidInvalidate(NFCNdefReaderSession session, NSError error)
            {
                // TODO: probably throw
            }
        }
    }

    static class NFCNdefMessageExtensions
    {
        public static byte[] ToByteArray(this NFCNdefMessage message)
        {
            var records = message?.Records;

            // Empty message: single empty record
            if (records == null || records.Length == 0)
            {
                records = new NFCNdefPayload[] { null };
            }

            var m = new MemoryStream();
            for (var i = 0; i < records.Length; i++)
            {
                var record = records[i];
                var typeNameFormat = record?.TypeNameFormat ?? NFCTypeNameFormat.Empty;
                var payload = record?.Payload;
                var id = record?.Identifier;
                var type = record?.Type;

                var flags = (byte)typeNameFormat;

                // Message begin / end flags. If there is only one record in the message, both flags are set.
                if (i == 0)
                    flags |= 0x80;      // MB (message begin = first record in the message)
                if (i == records.Length - 1)
                    flags |= 0x40;      // ME (message end = last record in the message)

                // cf (chunked records) not supported yet

                // SR (Short Record)?
                if (payload == null || payload.Length < 255)
                    flags |= 0x10;

                // ID present?
                if (id != null && id.Length > 0)
                    flags |= 0x08;

                m.WriteByte(flags);

                // Type length
                if (type != null)
                    m.WriteByte((byte)type.Length);
                else
                    m.WriteByte(0);

                // Payload length 1 byte (SR) or 4 bytes
                if (payload == null)
                {
                    m.WriteByte(0);
                }
                else
                {
                    if ((flags & 0x10) != 0)
                    {
                        // SR
                        m.WriteByte((byte)payload.Length);
                    }
                    else
                    {
                        // No SR (Short Record)
                        var payloadLength = (uint)payload.Length;
                        m.WriteByte((byte)(payloadLength >> 24));
                        m.WriteByte((byte)(payloadLength >> 16));
                        m.WriteByte((byte)(payloadLength >> 8));
                        m.WriteByte((byte)(payloadLength & 0x000000ff));
                    }
                }

                // ID length
                if (id != null && (flags & 0x08) != 0)
                    m.WriteByte((byte)id.Length);

                // Type length
                if (type != null && type.Length > 0)
                    m.Write(type.ToArray(), 0, (int)type.Length);

                // ID data
                if (id != null && id.Length > 0)
                    m.Write(id.ToArray(), 0, (int)id.Length);

                // Payload data
                if (payload != null && payload.Length > 0)
                    m.Write(payload.ToArray(), 0, (int)payload.Length);
            }

            return m.ToArray();
        }
    }
}
