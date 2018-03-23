using System;
using Windows.ApplicationModel.Calls;

namespace Microsoft.Caboodle
{
    public static partial class PhoneDialer
    {
        internal static bool IsSupported =>
             Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.ApplicationModel.Calls.PhoneCallManager");

        public static void Open(string number)
        {
            ValidateOpen(number);

            PhoneCallManager.ShowPhoneCallUI(number, string.Empty);
        }
    }
}
