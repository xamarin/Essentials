using System.Windows.Input;
using Microsoft.Caboodle;
using Xamarin.Forms;

namespace Caboodle.Samples.ViewModel
{
    public class SmsViewModel : BaseViewModel
    {
        string recipient;
        string messageText;

        public SmsViewModel()
        {
            SendSmsCommand = new Command(OnSendSms);
        }

        public string Recipient
        {
            get => recipient;
            set => base.SetProperty(ref recipient, value, onChanged: OnChange);
        }

        public string MessageText
        {
            get => messageText;
            set => SetProperty(ref messageText, value, onChanged: OnChange);
        }

        public ICommand SendSmsCommand { get; }

        public bool CanSend => IsSupported && IsValid;

        public bool IsSupported => Sms.IsComposeSupported;

        public bool IsValid => !string.IsNullOrWhiteSpace(MessageText) && !string.IsNullOrWhiteSpace(Recipient);

        private void OnChange()
        {
            OnPropertyChanged(nameof(CanSend));
            OnPropertyChanged(nameof(IsSupported));
            OnPropertyChanged(nameof(IsValid));
        }

        async void OnSendSms()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            var message = new SmsMessage(MessageText, Recipient);
            await Sms.ComposeAsync(message);

            IsBusy = false;
        }
    }
}
