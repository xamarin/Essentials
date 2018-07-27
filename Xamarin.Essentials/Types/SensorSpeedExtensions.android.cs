using System;
using System.Collections.Generic;
using System.Text;
using Android.Hardware;

namespace Xamarin.Essentials.Types
{
    public static class SensorSpeedExtensions
    {
        public static SensorDelay ToNative(this SensorSpeed sensorSpeed)
        {
            var delay = SensorDelay.Normal;
            switch (sensorSpeed)
            {
                case SensorSpeed.Normal:
                    delay = SensorDelay.Normal;
                    break;
                case SensorSpeed.Fastest:
                    delay = SensorDelay.Fastest;
                    break;
                case SensorSpeed.Game:
                    delay = SensorDelay.Game;
                    break;
                case SensorSpeed.UI:
                    delay = SensorDelay.Ui;
                    break;
            }
            return delay;
        }
    }
}
