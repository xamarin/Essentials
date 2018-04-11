using System;

namespace Xamarin.Essentials
{
    public static partial class Battery
    {
        private static event BatteryChangedEventHandler BatteryChanagedInternal;

        private static double currentLevel;

        private static BatteryPowerSource currentSource;

        private static BatteryState currentState;

        public static event BatteryChangedEventHandler BatteryChanged
        {
            add
            {
                var wasRunning = BatteryChanagedInternal != null;

                BatteryChanagedInternal += value;

                if (!wasRunning && BatteryChanagedInternal != null)
                {
                    SetCurrent();
                    StartBatteryListeners();
                }
            }

            remove
            {
                var wasRunning = BatteryChanagedInternal != null;

                BatteryChanagedInternal -= value;

                if (wasRunning && BatteryChanagedInternal == null)
                    StopBatteryListeners();
            }
        }

        private static void SetCurrent()
        {
            currentLevel = Battery.ChargeLevel;
            currentSource = Battery.PowerSource;
            currentState = Battery.State;
        }

        private static void OnBatteryChanged(double level, BatteryState state, BatteryPowerSource source)
            => OnBatteryChanged(new BatteryChangedEventArgs(level, state, source));

        private static void OnBatteryChanged()
            => OnBatteryChanged(ChargeLevel, State, PowerSource);

        private static void OnBatteryChanged(BatteryChangedEventArgs e)
        {
            if (currentLevel != e.ChargeLevel ||
                currentSource != e.PowerSource ||
                currentState != e.State)
            {
                SetCurrent();
                BatteryChanagedInternal?.Invoke(e);
            }
        }
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
