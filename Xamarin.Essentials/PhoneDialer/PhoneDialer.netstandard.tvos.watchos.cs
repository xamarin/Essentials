namespace Xamarin.Essentials
{
    public static partial class PhoneDialer
    {
        internal static bool IsSupported =>
            ThrowHelper.ThrowNotImplementedException<bool>();

        static void PlatformOpen(string number) =>
            ThrowHelper.ThrowNotImplementedException();
    }
}
