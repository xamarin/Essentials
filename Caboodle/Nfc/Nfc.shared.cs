using System;

namespace Microsoft.Caboodle
{
    public static partial class Nfc
    {
        static event NfcTagEventHandler TagArrivedInternal;

        static event NfcTagEventHandler TagDepartedInternal;

        static event NdefMessageReceivedEventHandler NdefMessageReceivedInternal;

        static bool HasTagListeners => TagArrivedInternal != null || TagDepartedInternal != null;

        public static event NfcTagEventHandler TagArrived
        {
            add
            {
                var wasRunning = HasTagListeners;
                TagArrivedInternal += value;
                if (!wasRunning && HasTagListeners)
                    StartTagListeners();
            }

            remove
            {
                var wasRunning = HasTagListeners;
                TagArrivedInternal -= value;
                if (wasRunning && !HasTagListeners)
                    StopTagListeners();
            }
        }

        public static event NfcTagEventHandler TagDeparted
        {
            add
            {
                var wasRunning = HasTagListeners;
                TagDepartedInternal += value;
                if (!wasRunning && HasTagListeners)
                    StartTagListeners();
            }

            remove
            {
                var wasRunning = HasTagListeners;
                TagDepartedInternal -= value;
                if (wasRunning && HasTagListeners)
                    StopTagListeners();
            }
        }

        public static event NdefMessageReceivedEventHandler NdefMessageReceived
        {
            add
            {
                var wasRunning = NdefMessageReceivedInternal != null;
                NdefMessageReceivedInternal += value;
                if (!wasRunning && NdefMessageReceivedInternal != null)
                    StartNdefMessageListeners();
            }

            remove
            {
                var wasRunning = NdefMessageReceivedInternal != null;
                NdefMessageReceivedInternal -= value;
                if (wasRunning && NdefMessageReceivedInternal == null)
                    StopNdefMessageListeners();
            }
        }

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
