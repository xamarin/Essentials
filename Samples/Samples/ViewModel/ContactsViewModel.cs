using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    class ContactsViewModel : BaseViewModel
    {
        string name;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        string phones;

        public string Phones
        {
            get => phones;
            set => SetProperty(ref phones, value);
        }

        string emails;

        public string Emails
        {
            get => emails;
            set => SetProperty(ref emails, value);
        }

        string contactType;

        public string ContactType
        {
            get => contactType;
            set => SetProperty(ref contactType, value);
        }

        ObservableCollection<Contact> contactsList = new ObservableCollection<Contact>();

        public ObservableCollection<Contact> ContactsList { get => contactsList; set => SetProperty(ref contactsList, value); }

        public ICommand GetContactCommand { get; }

        public ICommand GetAllContactCommand { get; }

        public ContactsViewModel()
        {
            GetContactCommand = new Command(OnGetContact);
            GetAllContactCommand = new Command(async () => await OnGetAllContact());
        }

        async void OnGetContact()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                Phones = string.Empty;
                Emails = string.Empty;
                Name = string.Empty;
                ContactType = string.Empty;

                var contact = await Contacts.PickContactAsync();
                if (contact == null)
                    return;

                foreach (var number in contact?.Numbers)
                    Phones += $"{number.PhoneNumber} ({number.ContactType})" + Environment.NewLine;

                foreach (var email in contact?.Emails)
                    Emails += $"{email.EmailAddress} ({email.ContactType})" + Environment.NewLine;

                Name = contact?.Name;
                ContactType = contact?.ContactType.ToString();
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () => await DisplayAlertAsync($"Error:{ex.Message}"));
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task OnGetAllContact()
        {
            if (DeviceInfo.Platform != DevicePlatform.iOS)
                await Permissions.RequestAsync<Permissions.ContactsRead>();
            GetAllContact();
        }

        void GetAllContact()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                var tasks = Contacts.GetAllAsync();
                if (tasks != null)
                {
                    foreach (var task in tasks)
                    {
                        Task.Run(async () =>
                        {
                             foreach (var contact in await task)
                                 MainThread.BeginInvokeOnMainThread(() => ContactsList.Add(contact));
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () => await DisplayAlertAsync($"Error:{ex.Message}"));
            }
            IsBusy = false;
        }
    }
}
