using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Caboodle;
using Xamarin.Forms;

namespace Caboodle.Samples.ViewModel
{
    public class EmailViewModel : BaseViewModel
    {
        string tomails;
        string ccmails;

        public EmailViewModel()
        {
            // For testing mailboxes (w/o registration) go to:
            // https://www.mailinator.com
            tomails = string.Join(",", new string[] { "caboodle@mailinator.com", "caboodle01@mailinator.com" });
            ccmails = string.Join(";", new string[] { "caboodle@mailinator.com", "caboodle02@mailinator.com" });
            RecipientsTo = tomails;
            RecipientsCC = ccmails;
            Subject = "Caboodle.Email Test message";
            Body = "This is an email from Caboodle.Email!";
 
            SendEmailCommand = new Command(OnSendEmail);

            return;
        }

        public ICommand SendEmailCommand { get; }

        string subject;

        public string Subject
        {
            get => subject;
            set => SetProperty(ref subject, value);
        }

        string body;

        public string Body
        {
            get => body;
            set => SetProperty(ref body, value);
        }

        string recipientsto;

        public string RecipientsTo
        {
            get => recipientsto;
            set => SetProperty(ref recipientsto, value);
        }

        string recipientscc;

        public string RecipientsCC
        {
            get => recipientscc;
            set => SetProperty(ref recipientscc, value);
        }

        string recipientsbcc;

        public string RecipientsBCC
        {
            get => recipientsbcc;
            set => SetProperty(ref recipientsbcc, value);
        }

        private async void OnSendEmail()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            try
            {
                await Email.ComposeAsync(
                     RecipientsTo?.Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries),
                     RecipientsCC?.Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries),
                     RecipientsBCC?.Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries),
                     Subject,
                     Body,
                     "text/plain",
                     null);
            }
            catch (Exception)
            {
            }
            finally
            {
                IsBusy = false;
            }

            return;
        }
    }
}
