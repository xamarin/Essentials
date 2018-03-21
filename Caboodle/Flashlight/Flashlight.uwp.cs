using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Devices.Enumeration;
using Windows.Devices.Lights;

namespace Microsoft.Caboodle
{
    public static partial class Flashlight
    {
        static Lamp lamp;

        /// <summary>
        /// Flashlight constructor
        /// </summary>
        /// <see cref="https://docs.microsoft.com/en-us/windows/uwp/audio-video-camera/camera-independent-flashlight"/>
        static Flashlight()
        {
            InitializeComponent();

            return;
        }

        private static void InitializeComponent()
        {
            var task1 = Task.Run<Lamp>(async () => await Lamp.GetDefaultAsync());

            var task2 = Task.Run<Lamp>(async () =>
            {
                var selectorString = Lamp.GetDeviceSelector();
                var devices = Task.Run(async () => await DeviceInformation.FindAllAsync(selectorString));

                var deviceInfo =
                    devices.Result.FirstOrDefault(di => di.EnclosureLocation != null &&
                        di.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back);

                if (deviceInfo == null)
                {
                    throw new FlashlightException();
                }

                return await Lamp.FromIdAsync(deviceInfo.Id);
            });

            if (task1.Result != null)
            {
                lamp = task1.Result;
            }
            else
            {
                if (task2.Result != null)
                {
                    lamp = task2.Result;
                }
                else
                {
                    throw new FlashlightException("Flashlight/Lamp not found or cannot be used");
                }
            }

            return;
        }

        public static void On()
        {
            lamp.IsEnabled = true;

            return;
        }

        public static void Off()
        {
            lamp.IsEnabled = false;

            return;
        }
    }
}
