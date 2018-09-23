using UIKit;
using UserNotifications;

namespace Xamarin.Essentials
{
    public static partial class Notifications
    {
        internal static bool AlwaysUseUILocalNotification { get; set; } = false;

        static void PlatformShow(Notification notification)
        {
            Permissions.CheckStatusAsync(PermissionType.Notifications);

            // get the values we can understand
            var title = string.IsNullOrEmpty(notification.Title)
                ? null
                : notification.Title;
            var description = string.IsNullOrEmpty(notification.Description)
                ? null
                : notification.Description;

            if (Platform.HasOSVersion(10, 0) && !AlwaysUseUILocalNotification)
            {
            }
            else
            {
                // cancel the previous notification
                PlatformCancel(notification);

                // create the new notification
                var nativeNotification = new UILocalNotification
                {
                    SoundName = UILocalNotification.DefaultSoundName,
                    AlertTitle = title,
                    AlertBody = description
                };

                // show it
                MainThread.BeginInvokeOnMainThread(() => UIApplication.SharedApplication.PresentLocalNotificationNow(nativeNotification));
            }
        }

        static void PlatformCancel(Notification notification)
        {
            if (Platform.HasOSVersion(10, 0) && !AlwaysUseUILocalNotification)
            {
                var id = GetNativeId(notification.Id);
                UNUserNotificationCenter.Current.RemoveDeliveredNotifications(new[] { id });
            }
            else
            {
                // cancel the native notification if we can
                var native = notification.NativeNotification;
                if (native != null)
                    MainThread.BeginInvokeOnMainThread(() => UIApplication.SharedApplication.CancelLocalNotification(native));
            }
        }

        static void PlatformCancel(int notificationId)
        {
            if (Platform.HasOSVersion(10, 0) && !AlwaysUseUILocalNotification)
            {
                var id = GetNativeId(notificationId);
                UNUserNotificationCenter.Current.RemoveDeliveredNotifications(new[] { id });
            }
            else
            {
                // TODO: not supported
            }
        }

        static void PlatformCancelAll()
        {
            if (Platform.HasOSVersion(10, 0) && !AlwaysUseUILocalNotification)
            {
                UNUserNotificationCenter.Current.RemoveAllDeliveredNotifications();
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() => UIApplication.SharedApplication.CancelAllLocalNotifications());
            }
        }

        static string GetNativeId(int notificationId) => notificationId.ToString();
    }

    public partial class Notification
    {
        internal UILocalNotification NativeNotification { get; set; }
    }
}
