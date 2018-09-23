using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    class NotificationsViewModel : BaseViewModel
    {
        int lastId = 0;
        string title = AppInfo.Name;
        string description = "This is a notification description right here!";

        public NotificationsViewModel()
        {
            ShowCommand = new Command(OnShow);
            UpdateCommand = new Command(OnUpdate);
            CancelCommand = new Command(OnCancel);
            ClearCommand = new Command(OnClear);
        }

        public ICommand ShowCommand { get; }

        public ICommand UpdateCommand { get; }

        public ICommand CancelCommand { get; }

        public ICommand ClearCommand { get; }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        void OnShow()
        {
            var notification = new Notification
            {
                Id = ++lastId,
                Title = Title,
                Description = Description
            };

            try
            {
                Notifications.Show(notification);
            }
            catch (Exception ex)
            {
                DisplayAlertAsync($"Unable to show notification: {ex.Message}");
            }
        }

        void OnUpdate()
        {
            var notification = new Notification
            {
                Id = lastId,
                Title = Title,
                Description = Description
            };

            try
            {
                Notifications.Show(notification);
            }
            catch (Exception ex)
            {
                DisplayAlertAsync($"Unable to update notification: {ex.Message}");
            }
        }

        void OnCancel()
        {
            try
            {
                Notifications.Cancel(lastId);
            }
            catch (Exception ex)
            {
                DisplayAlertAsync($"Unable to cancel notification: {ex.Message}");
            }
        }

        void OnClear()
        {
            try
            {
                Notifications.CancelAll();
            }
            catch (Exception ex)
            {
                DisplayAlertAsync($"Unable to cancel notifications: {ex.Message}");
            }
        }
    }
}
