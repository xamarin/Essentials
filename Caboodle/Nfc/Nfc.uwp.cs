using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Networking.Proximity;
using Windows.Storage.Streams;

namespace Microsoft.Caboodle
{
    public static partial class Nfc
    {
        static long ndefSubscriptionId = -1;
        static long publishedMessageId = -1;

        static ProximityDevice DefaultDevice => ProximityDevice.GetDefault();

        internal static bool IsSupported => DefaultDevice != null;

        internal static bool IsEnabled
        {
            get
            {
                if (IsSupported)
                {
                    try
                    {
                        return DefaultDevice?.MaxMessageBytes > 0;
                    }
                    catch
                    {
                    }
                }

                return false;
            }
        }

        static void ValidateDeviceState()
        {
            if (!IsSupported)
                throw new NotSupportedException();

            if (!IsEnabled)
                throw new NotSupportedException();
        }

        static void PlatformPublishMessage(string messageType, byte[] message)
        {
            ValidateDeviceState();

            StopPublishing();

            var dataWriter = new DataWriter
            {
                UnicodeEncoding = UnicodeEncoding.Utf16LE
            };
            dataWriter.WriteBytes(message);

            publishedMessageId = DefaultDevice.PublishBinaryMessage(
                messageType, dataWriter.DetachBuffer(), (d, msg) => StopPublishing());
        }

        static void PlatformStopPublishing()
        {
            ValidateDeviceState();

            if (publishedMessageId != -1)
            {
                DefaultDevice.StopPublishingMessage(publishedMessageId);
                publishedMessageId = -1;
            }
        }

        static void StartTagListeners()
        {
            ValidateDeviceState();

            DefaultDevice.DeviceArrived += OnDeviceArrived;
            DefaultDevice.DeviceDeparted += OnDeviceDeparted;
        }

        static void StopTagListeners()
        {
            ValidateDeviceState();

            DefaultDevice.DeviceArrived -= OnDeviceArrived;
            DefaultDevice.DeviceDeparted -= OnDeviceDeparted;
        }

        static void OnDeviceArrived(ProximityDevice sender)
        {
            TagArrivedInternal?.Invoke(NfcTagEventArgs.Empty);
        }

        static void OnDeviceDeparted(ProximityDevice sender)
        {
            TagDepartedInternal?.Invoke(NfcTagEventArgs.Empty);
        }

        static void StartNdefMessageListeners()
        {
            ValidateDeviceState();

            if (ndefSubscriptionId == -1)
            {
                ndefSubscriptionId = DefaultDevice.SubscribeForMessage("NDEF", OnNdefMessageReceived);
            }
        }

        static void StopNdefMessageListeners()
        {
            ValidateDeviceState();

            if (ndefSubscriptionId != -1)
            {
                DefaultDevice.StopSubscribingForMessage(ndefSubscriptionId);
                ndefSubscriptionId = -1;
            }
        }

        static void OnNdefMessageReceived(ProximityDevice sender, ProximityMessage message)
        {
            var bytes = message.Data.ToArray();
            NdefMessageReceivedInternal?.Invoke(new NdefMessageReceivedEventArgs(bytes));
        }
    }
}
