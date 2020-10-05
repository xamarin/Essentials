using System;

namespace Xamarin.Essentials
{
    public static partial class PhoneDialer
    {
        internal static void ValidateOpen(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                ThrowHelper.ThrowArgumentNullException(nameof(number));

            if (!IsSupported)
               ThrowHelper.ThrowNotImplementedException();
        }

        public static void Open(string number)
            => PlatformOpen(number);
    }
}
