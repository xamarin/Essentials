using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Samples.ViewModel
{
    public class ContactViewModel : BaseViewModel
    {
        string name;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        string birthday;

        public string Birthday
        {
            get => birthday;
            set => SetProperty(ref birthday, value);
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

        string contactName;

        public string ContactName
        {
            get => contactName;
            set => SetProperty(ref contactName, value);
        }

        string contactNumber;

        public string ContactNumber
        {
            get => contactNumber;
            set => SetProperty(ref contactNumber, value);
        }

        public ICommand GetContactCommand { get; }

        public ICommand SaveContactCommand { get; }

        public ContactViewModel()
        {
            GetContactCommand = new Command(OnGetContact);
            SaveContactCommand = new Command(OnSaveContact);
        }

        async void OnSaveContact()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                await Contact.SaveContactAsync(ContactName, ContactNumber);

                ContactName = string.Empty;
                ContactNumber = string.Empty;
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

        async void OnGetContact()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                var contact = await Contact.PickContactAsync();
                contact.Numbers.ForEach(x => Phones += x.Key + Environment.NewLine);
                contact.Emails.ForEach(x => Emails += x.Key + Environment.NewLine);
                Name = contact.Name;
                Birthday = contact.Birthday;
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
    }
}
