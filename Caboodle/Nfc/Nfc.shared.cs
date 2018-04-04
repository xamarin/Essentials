using System;

namespace Microsoft.Caboodle
{
    public static partial class Nfc
    {
        static bool isListening;

        public static bool IsListening
        {
            get => isListening;
            set
            {
                if (isListening != value)
                {
                    isListening = value;
                    if (isListening)
                    {
                        StartTagListeners();
                        StartNdefMessageListeners();
                    }
                    else
                    {
                        StopNdefMessageListeners();
                        StopTagListeners();
                    }
                }
            }
        }

        public static event NfcTagEventHandler TagArrived;

        public static event NfcTagEventHandler TagDeparted;

        public static event NdefMessageReceivedEventHandler NdefMessageReceived;

        public static void PublishMessage(string messageType, byte[] message)
            => PlatformPublishMessage(messageType, message);

        public static void WriteTag(byte[] message)
            => PublishMessage("NDEF:WriteTag", message);

        public static void ShareTag(byte[] message)
            => PublishMessage("NDEF", message);

        public static void StopPublishing()
            => PlatformStopPublishing();
    }

    public delegate void NfcTagEventHandler(NfcTagEventArgs e);

    public delegate void NdefMessageReceivedEventHandler(NdefMessageReceivedEventArgs e);

    public class NfcTagEventArgs : EventArgs
    {
        public static new readonly NfcTagEventArgs Empty = new NfcTagEventArgs();

        public NfcTagEventArgs()
        {
        }
    }

    public class NdefMessageReceivedEventArgs : EventArgs
    {
        public NdefMessageReceivedEventArgs(byte[] message)
        {
            Message = message;
        }

        public byte[] Message { get; }
    }
}
