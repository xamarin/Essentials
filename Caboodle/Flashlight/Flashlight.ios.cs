using System.Threading.Tasks;
using AVFoundation;

namespace Microsoft.Caboodle
{
    public static partial class Flashlight
    {
        static AVCaptureDevice device;
        static bool hasLoadedDevice;

        public static Task TurnOnAsync()
        {
            FindDevice();
            if (device == null)
                throw new FeatureNotSupportedException();

            device.SetTorchModeLevel(AVCaptureDevice.MaxAvailableTorchLevel, out var error);
            device.FlashMode = AVCaptureFlashMode.On;

            return Task.CompletedTask;
        }

        public static Task TurnOffAsync()
        {
            if (device != null)
            {
                device.TorchMode = AVCaptureTorchMode.Off;
                device.FlashMode = AVCaptureFlashMode.Off;
            }

            return Task.CompletedTask;
        }

        static void FindDevice()
        {
            if (hasLoadedDevice)
                return;

            var captureDevice = AVCaptureDevice.GetDefaultDevice(AVMediaType.Video);
            if (captureDevice != null)
            {
                captureDevice.LockForConfiguration(out var error);
                if (error == null)
                {
                    device = captureDevice;
                }
                captureDevice.UnlockForConfiguration();
            }

            hasLoadedDevice = true;
        }
    }
}
