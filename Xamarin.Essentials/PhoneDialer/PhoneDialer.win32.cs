namespace Xamarin.Essentials
{
    public static partial class PhoneDialer
    {
        internal static bool IsSupported => true;

        static void PlatformOpen(string number)
        {
            ValidateOpen(number);

            Launcher.OpenAsync("tel:" + number);
        }
    }
}
