namespace Xamarin.Essentials
{
    public static partial class Battery
    {
        static void StartBatteryListeners() =>
            ThrowHelper.ThrowNotImplementedException();

        static void StopBatteryListeners() =>
            ThrowHelper.ThrowNotImplementedException();

        static double PlatformChargeLevel =>
            ThrowHelper.ThrowNotImplementedException<double>();

        static BatteryState PlatformState =>
            ThrowHelper.ThrowNotImplementedException<BatteryState>();

        static BatteryPowerSource PlatformPowerSource =>
            ThrowHelper.ThrowNotImplementedException<BatteryPowerSource>();

        static void StartEnergySaverListeners() =>
            ThrowHelper.ThrowNotImplementedException();

        static void StopEnergySaverListeners() =>
            ThrowHelper.ThrowNotImplementedException();

        static EnergySaverStatus PlatformEnergySaverStatus =>
            ThrowHelper.ThrowNotImplementedException<EnergySaverStatus>();
    }
}
