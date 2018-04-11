namespace Xamarin.Essentials
{
    public static partial class Compass
    {
        internal static bool IsSupported =>
            throw new NotImplementedInReferenceAssemblyException();

        private static void PlatformStart(SensorSpeed sensorSpeed) =>
            throw new NotImplementedInReferenceAssemblyException();

        private static void PlatformStop() =>
            throw new NotImplementedInReferenceAssemblyException();
    }
}
