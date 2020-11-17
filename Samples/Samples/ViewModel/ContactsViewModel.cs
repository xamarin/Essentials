using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    class ContactsViewModel : BaseViewModel
    {
        ObservableCollection<Contact> contactsList = new ObservableCollection<Contact>();
        int count = 0;
        Contact selectedContact;

        public ContactsViewModel()
        {
            GetContactCommand = new Command(OnGetContact);
            GetAllContactCommand = new Command(OnGetAllContact);
        }

        public ObservableCollection<Contact> ContactsList
        {
            get => contactsList;
            set => SetProperty(ref contactsList, value);
        }

        public int Count
        {
            get => count;
            set => SetProperty(ref count, value);
        }

        public Contact SelectedContact
        {
            get => selectedContact;
            set => SetProperty(ref selectedContact, value, onChanged: OnContactSelected);
        }

        public ICommand GetContactCommand { get; }

        public ICommand GetAllContactCommand { get; }

        async void OnGetContact()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                var contact = await Contacts.PickContactAsync();
                if (contact == null)
                    return;

                var details = new ContactDetailsViewModel(contact);
                await NavigateAsync(details);
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

        async void OnGetAllContact()
        {
            if (await Permissions.RequestAsync<Permissions.ContactsRead>() != PermissionStatus.Granted)
                return;

            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                Count = 0;
                ContactsList?.Clear();

                await Task.Run(async () =>
                {
                    var contacts = await Contacts.GetAllAsync();

                    foreach (var contact in contacts)
                    {
                        await MainThread.InvokeOnMainThreadAsync(() => ContactsList.Add(contact));
                        Count++;
                    }
                });
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync($"Error:{ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async void OnContactSelected()
        {
            if (SelectedContact == null)
                return;

            var details = new ContactDetailsViewModel(SelectedContact);

            SelectedContact = null;

            await NavigateAsync(details);
        }
    }
}
