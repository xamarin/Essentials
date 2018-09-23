namespace Xamarin.Essentials
{
    public static partial class Notifications
    {
        static void PlatformShow(Notification notification) =>
            throw new NotImplementedInReferenceAssemblyException();

        static void PlatformCancel(int notificationId) =>
            throw new NotImplementedInReferenceAssemblyException();

        static void PlatformCancel(Notification notification) =>
            throw new NotImplementedInReferenceAssemblyException();

        static void PlatformCancelAll() =>
            throw new NotImplementedInReferenceAssemblyException();
    }
}
