using System;
using Windows.ApplicationModel.Calls;

namespace Microsoft.Caboodle
{
    public static partial class PhoneDialer
    {
        public static bool IsSupported =>
             Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.ApplicationModel.Calls.PhoneCallManager");

        public static void Open(string number) => Open(number, null);

        public static void Open(string number, string name)
        {
            ValidateOpen(number);

            PhoneCallManager.ShowPhoneCallUI(number, name ?? string.Empty);
        }
    }
}
