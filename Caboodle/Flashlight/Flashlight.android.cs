using System;
using Android;
using Android.Content;
using Android.Content.PM;
using Android.Hardware.Camera2;
using Android.OS;

namespace Microsoft.Caboodle
{
    public static partial class Flashlight
    {
        private static CameraManager manager;
        private static CameraCharacteristics characteristics;
        private static string cameraid;
        private static bool iscamerainfoavailable;
        private static bool isfeaturecameraflashavailable;

        /// <summary>
        /// Flashlight constructor
        /// </summary>
        static Flashlight()
        {
            InitializeComponent();

            return;
        }

        public static bool IsAvailable
        {
            get
            {
                return isfeaturecameraflashavailable && iscamerainfoavailable;
            }
        }

        private static void InitializeComponent()
        {
            isfeaturecameraflashavailable = Platform.CurrentActivity.ApplicationContext.
                        PackageManager.HasSystemFeature(PackageManager.FeatureCameraFlash);
            manager = Platform.CurrentActivity.GetSystemService(Context.CameraService) as CameraManager;
            cameraid = manager.GetCameraIdList()[0];
            characteristics = manager.GetCameraCharacteristics(cameraid);
            iscamerainfoavailable = (bool)characteristics.Get(CameraCharacteristics.FlashInfoAvailable);

            return;
        }

        private static bool flashon;

        public static void On()
        {
            if (cameraid.Equals("1"))
            {
                // camera back
                if (!flashon)
                {
                    try
                    {
                        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                        {
                            manager.SetTorchMode(cameraid, true);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new FlashlightException($"Flashlight On failed: {e.Message}");
                    }
                    flashon = true;
                }
            }

            return;
        }

        public static void Off()
        {
            if (cameraid.Equals("1"))
            {
                // camera back
                if (!flashon)
                {
                    try
                    {
                        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                        {
                            manager.SetTorchMode(cameraid, true);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new FlashlightException($"Flashlight On failed: {e.Message}");
                    }
                    flashon = true;
                }
            }

            return;
        }
    }
}
