using System;

namespace Xamarin.Essentials
{
    public static partial class Battery
    {
        static event BatteryChangedEventHandler BatteryChanagedInternal;

        public static event BatteryChangedEventHandler BatteryChanged
        {
            add
            {
                var wasRunning = BatteryChanagedInternal != null;

                BatteryChanagedInternal += value;

                if (!wasRunning && BatteryChanagedInternal != null)
                    StartBatteryListeners();
            }

            remove
            {
                var wasRunning = BatteryChanagedInternal != null;

                BatteryChanagedInternal -= value;

                if (wasRunning && BatteryChanagedInternal == null)
                    StopBatteryListeners();
            }
        }

        static void OnBatteryChanged(double level, BatteryState state, BatteryPowerSource source)
            => OnBatteryChanged(new BatteryChangedEventArgs(level, state, source));

        static void OnBatteryChanged()
            => OnBatteryChanged(ChargeLevel, State, PowerSource);

        static void OnBatteryChanged(BatteryChangedEventArgs e)
            => BatteryChanagedInternal?.Invoke(e);
    }

    public enum BatteryState
    {
        Unknown,
        Charging,
        Discharging,
        Full,
        NotCharging,
        NotPresent
    }

    public enum BatteryPowerSource
    {
        Unknown,
        Battery,
        Ac,
        Usb,
        Wireless
    }

    public delegate void BatteryChangedEventHandler(BatteryChangedEventArgs e);

    public class BatteryChangedEventArgs : EventArgs
    {
        internal BatteryChangedEventArgs(double level, BatteryState state, BatteryPowerSource source)
        {
            ChargeLevel = level;
            State = state;
            PowerSource = source;
        }

        public double ChargeLevel { get; }

        public BatteryState State { get; }

        public BatteryPowerSource PowerSource { get; }
    }
}
