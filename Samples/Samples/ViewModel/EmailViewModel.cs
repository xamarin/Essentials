using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class EmailViewModel : BaseViewModel
    {
        private string subject;
        private string body;
        private string recipientsTo;
        private string recipientsCc;
        private string recipientsBcc;

        public EmailViewModel()
        {
            SendEmailCommand = new Command(OnSendEmail);
        }

        public ICommand SendEmailCommand { get; }

        public string Subject
        {
            get => subject;
            set => SetProperty(ref subject, value);
        }

        public string Body
        {
            get => body;
            set => SetProperty(ref body, value);
        }

        public string RecipientsTo
        {
            get => recipientsTo;
            set => SetProperty(ref recipientsTo, value);
        }

        public string RecipientsCc
        {
            get => recipientsCc;
            set => SetProperty(ref recipientsCc, value);
        }

        public string RecipientsBcc
        {
            get => recipientsBcc;
            set => SetProperty(ref recipientsBcc, value);
        }

        private async void OnSendEmail()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Email.ComposeAsync(new EmailMessage
                {
                    Subject = Subject,
                    Body = Body,
                    To = Split(RecipientsTo),
                    Cc = Split(RecipientsCc),
                    Bcc = Split(RecipientsBcc),
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        private List<string> Split(string recipients)
            => recipients?.Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)?.ToList();
    }
}
