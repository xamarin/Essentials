using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        ObservableCollection<string> contactsList = new ObservableCollection<string>();

        public ObservableCollection<string> ContactsList { get => contactsList; set => SetProperty(ref contactsList, value); }

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

                var contact = await Contacts.PickContactAsync();
                if (contact == null)
                    return;

                foreach (var number in contact?.Phones)
                {
                    Phones += $"{number.Value}{Environment.NewLine}({number.Type})"
                        + Environment.NewLine + Environment.NewLine;
                }

                foreach (var email in contact?.Emails)
                {
                    Emails += $"{email.Value}{Environment.NewLine}({email.Type})"
                        + Environment.NewLine + Environment.NewLine;
                }

                Name = contact?.Name;
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
            if (await Permissions.RequestAsync<Permissions.ContactsRead>() != PermissionStatus.Granted)
                return;

            GetAllContact();
        }

        async void GetAllContact()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            ContactsList?.Clear();
            try
            {
                await Task.Run(async () =>
                {
                    var contacts = await Contacts.GetAllAsync();

                    foreach (var contact in contacts)
                    {
                        var c =
                            $"{contact.Name}" +
                            $" {contact.Phones?.FirstOrDefault()?.Value} " +
                            $"({contact.Phones?.FirstOrDefault()?.Type})";

                        await MainThread.InvokeOnMainThreadAsync(() => ContactsList.Add(c));
                    }
                });
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync($"Error:{ex.Message}");
            }
            IsBusy = false;
        }
    }
}
