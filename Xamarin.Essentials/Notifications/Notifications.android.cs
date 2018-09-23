using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;

namespace Xamarin.Essentials
{
    public static partial class Notifications
    {
        const int defaultSmallIcon = global::Android.Resource.Drawable.IcDialogInfo;
#if __ANDROID_26__
        const string defaultChannelName = "Notifications";
        const string channelId = "xamarinessentials";
#endif

        public static int SmallIconResource { get; set; }

        public static string ChannelName { get; set; }

        public static string ChannelDescription { get; set; }

        static void PlatformShow(Notification notification)
        {
            // get the values we can understand
            var title = string.IsNullOrEmpty(notification.Title)
                ? null
                : notification.Title;
            var description = string.IsNullOrEmpty(notification.Description)
                ? null
                : notification.Description;

            // start building
            var builder = new NotificationCompat.Builder(Platform.AppContext);
            builder.SetPriority(NotificationCompat.PriorityDefault);

            // lauch the app when the notification is tapped
            var launchIntent = Platform.AppContext.PackageManager.GetLaunchIntentForPackage(Platform.AppContext.PackageName);
            if (launchIntent != null)
            {
                var pendingIntent = PendingIntent.GetActivity(
                    Platform.AppContext, notification.Id, launchIntent, PendingIntentFlags.CancelCurrent);
                builder.SetContentIntent(pendingIntent);
            }

            // set the small icon
            if (SmallIconResource == 0)
                builder.SetSmallIcon(defaultSmallIcon);
            else
                builder.SetSmallIcon(SmallIconResource);

#if __ANDROID_26__
            // create the channel for the newer OS versions
            if (Platform.HasApiLevel(BuildVersionCodes.O))
            {
                var nativeManager = NotificationManager.FromContext(Platform.AppContext);
                if (nativeManager.GetNotificationChannel(channelId) == null)
                {
                    var channelName = string.IsNullOrEmpty(ChannelName)
                        ? defaultChannelName
                        : ChannelName;

                    var channel = new NotificationChannel(channelId, channelName, NotificationImportance.Default);

                    if (!string.IsNullOrEmpty(ChannelDescription))
                        channel.Description = ChannelDescription;

                    nativeManager.CreateNotificationChannel(channel);
                }

                builder.SetChannelId(channelId);
            }
#endif

            // add the content
            if (title != null)
                builder.SetContentTitle(title);
            if (description != null)
                builder.SetContentText(description);

            // show the notification
            var nativeNotification = builder.Build();
            var manager = NotificationManagerCompat.From(Platform.AppContext);
            manager.Notify(notification.Id, nativeNotification);
        }

        static void PlatformCancel(int notificationId)
        {
            var manager = NotificationManagerCompat.From(Platform.AppContext);
            manager.Cancel(notificationId);
        }

        static void PlatformCancel(Notification notification) =>
            PlatformCancel(notification.Id);

        static void PlatformCancelAll()
        {
            var manager = NotificationManagerCompat.From(Platform.AppContext);
            manager.CancelAll();
        }
    }
}
