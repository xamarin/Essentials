using Windows.UI.Notifications;

namespace Xamarin.Essentials
{
    public static partial class Notifications
    {
        static void PlatformShow(Notification notification)
        {
            // remove any existing notification with this id
            PlatformCancel(notification.Id);

            // get the values we can understand
            var title = string.IsNullOrEmpty(notification.Title)
                ? null
                : notification.Title;
            var description = string.IsNullOrEmpty(notification.Description)
                ? null
                : notification.Description;

            // determine what template we want
            var template = title == null || description == null
                ? ToastTemplateType.ToastText01 // 1x title + 2x description
                : ToastTemplateType.ToastText02; // 3x description

            // get the template elements
            var toastContent = ToastNotificationManager.GetTemplateContent(template);
            var text1 = toastContent.SelectSingleNode("//text[@id='1']");
            var text2 = toastContent.SelectSingleNode("//text[@id='2']");

            // set toast text
            if (title == null || description == null)
            {
                text1.InnerText = title ?? description;
            }
            else
            {
                text1.InnerText = title;
                text2.InnerText = description;
            }

            // show the toast
            var toastNotifier = ToastNotificationManager.CreateToastNotifier();
            var toast = new ToastNotification(toastContent);
            toast.Tag = GetNativeId(notification.Id);
            toastNotifier.Show(toast);
        }

        static void PlatformCancel(int notificationId)
        {
            var tag = GetNativeId(notificationId);
            var history = ToastNotificationManager.History;
            history.Remove(tag);
        }

        static void PlatformCancel(Notification notification) =>
            PlatformCancel(notification.Id);

        static void PlatformCancelAll()
        {
            var history = ToastNotificationManager.History;
            history.Clear();
        }

        static string GetNativeId(int notificationId) => notificationId.ToString();
    }
}
