namespace Microsoft.Caboodle
{
    public static partial class Sms
    {
        static bool GetCanSendSms() => throw new NotImplentedInReferenceAssembly();

        static bool GetCanSendSmsInBackground() => throw new NotImplentedInReferenceAssembly();

        public static void PlatformSendSms(string recipient, string message) =>
            throw new NotImplentedInReferenceAssembly();

        public static void PlatformSendSmsInBackground(string recipient, string message) => throw new NotImplentedInReferenceAssembly();
    }
}
