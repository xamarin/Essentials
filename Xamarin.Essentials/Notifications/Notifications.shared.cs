using System;

namespace Xamarin.Essentials
{
    public static partial class Notifications
    {
        public static void Show(string description) =>
            Show(new Notification(description));

        public static void Show(string title, string description) =>
            Show(new Notification(title, description));

        public static void Show(int id, string title, string description) =>
            Show(new Notification(id, title, description));

        public static void Show(Notification notification)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            if (!notification.HasContent)
                throw new ArgumentException("The notification does not have any content.", nameof(notification));

            PlatformShow(notification);
        }

        public static void Cancel(int notificationId) =>
            PlatformCancel(notificationId);

        public static void Cancel(Notification notification) =>
            PlatformCancel(notification);

        public static void CancelAll() =>
            PlatformCancelAll();
    }

    public partial class Notification
    {
        public Notification()
            : this(0, null, null)
        {
        }

        public Notification(string description)
            : this(0, null, description)
        {
        }

        public Notification(string title, string description)
            : this(0, title, description)
        {
        }

        public Notification(int id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        internal bool HasContent =>
            !string.IsNullOrEmpty(Title) || !string.IsNullOrEmpty(Description);
    }
}
