using System.Threading.Tasks;
using Android;
using Android.Content.PM;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.OS;

using Camera = Android.Hardware.Camera;

namespace Microsoft.Caboodle
{
    public static partial class Flashlight
    {
        static readonly object locker = new object();
        static bool hasCameraPermission;

#pragma warning disable CS0618
        static Camera camera;
#pragma warning restore CS0618

        internal static bool IsSupported
            => Platform.HasSystemFeature(PackageManager.FeatureCameraFlash);

        public static async Task TurnOnAsync()
        {
            ValidateCameraPermission();

            if (!IsSupported)
                throw new FeatureNotSupportedException();

            await ToggleTorchAsync(true);
        }

        public static async Task TurnOffAsync()
        {
            ValidateCameraPermission();

            if (!IsSupported)
                throw new FeatureNotSupportedException();

            await ToggleTorchAsync(false);
            await ReleaseCameraAsync();
        }

        static void ValidateCameraPermission()
        {
            // TODO: request camera permissions for API Level >= 23

            if (hasCameraPermission)
                return;

            var permission = Manifest.Permission.Camera;
            if (!Platform.HasPermissionInManifest(permission))
                throw new PermissionException(permission);

            hasCameraPermission = true;
        }

        static Task ToggleTorchAsync(bool switchOn)
        {
            return Task.Run(() =>
            {
                if (Platform.HasApiLevel(BuildVersionCodes.M))
                {
                    var cameraManager = Platform.CameraManager;
                    foreach (var id in cameraManager.GetCameraIdList())
                    {
                        var hasFlash = cameraManager.GetCameraCharacteristics(id).Get(CameraCharacteristics.FlashInfoAvailable);
                        if (Java.Lang.Boolean.True.Equals(hasFlash))
                        {
                            cameraManager.SetTorchMode(id, switchOn);
                            break;
                        }
                    }
                }
                else
                {
                    lock (locker)
                    {
                        if (camera == null)
                        {
                            var camera = Camera.Open();
                            if (Platform.HasApiLevel(BuildVersionCodes.Honeycomb))
                            {
                                // required for (at least) the Nexus 5
                                camera.SetPreviewTexture(new SurfaceTexture(0));
                            }
                        }

                        var param = camera.GetParameters();
#pragma warning disable CS0618
                        param.FlashMode = switchOn ? Camera.Parameters.FlashModeTorch : Camera.Parameters.FlashModeOff;
#pragma warning restore CS0618
                        camera.SetParameters(param);
                        camera.StartPreview();
                    }
                }
            });
        }

        static async Task ReleaseCameraAsync()
        {
            await Task.Run(() =>
            {
                lock (locker)
                {
                    if (camera != null)
                    {
                        camera.StopPreview();
                        camera.SetPreviewCallback(null);
                        camera.Unlock();
                        camera.Release();
                        camera.Dispose();
                        camera = null;
                    }
                }
            });
        }
    }
}
