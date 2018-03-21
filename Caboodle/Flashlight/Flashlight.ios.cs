using AVFoundation;
using Foundation;
using UIKit;

namespace Microsoft.Caboodle
{
    public static partial class Flashlight
    {
        /// <summary>
        /// Flashlight constructor
        /// </summary>
        static Flashlight()
        {
            InitializeComponent();

            return;
        }

        private static AVCaptureDevice device;

        private static void InitializeComponent()
        {
            device = AVCaptureDevice.GetDefaultDevice(AVMediaType.Video);
            if (device == null)
            {
                throw new FlashlightException("Flashlight/Lamp not found or cannot be used");
            }

            NSError error = null;
            device.LockForConfiguration(out error);
            if (error != null)
            {
                device.UnlockForConfiguration();
                return;
            }

            return;
        }

        public static void On()
        {
            device.TorchMode = AVCaptureTorchMode.On;
            device.UnlockForConfiguration();

            return;
        }

        public static void Off()
        {
            device.TorchMode = AVCaptureTorchMode.Off;
            device.UnlockForConfiguration();

            return;
        }
    }
}
