using System.Windows.Input;
using Microsoft.Caboodle;
using Xamarin.Forms;

namespace Caboodle.Samples.ViewModel
{
    public class SmsViewModel : BaseViewModel
    {
        string recipient;
        string msgtext;

        public SmsViewModel()
        {
            SendSmsCommand = new Command(OnSendSms);
            SendSmsInBackgroundCommand = new Command(OnSendSmsInBackground);
        }

        public string Recipient
        {
            get => recipient;
            set => SetProperty(ref recipient, value);
        }

        public string MsgText
        {
            get => msgtext;
            set => SetProperty(ref msgtext, value);
        }

        public ICommand SendSmsCommand { get; }

        public ICommand SendSmsInBackgroundCommand { get; }

        public bool CanSendSms => Sms.CanSendSms;

        public bool CanSendSmsInBackground => Sms.CanSendSmsInBackground;

        void OnSendSms()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            Sms.SendSms(Recipient, MsgText, SmsSendType.Foreground);
            IsBusy = false;
        }

        void OnSendSmsInBackground()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            Sms.SendSms(Recipient, MsgText, SmsSendType.PreferBackground);
            IsBusy = false;
        }
    }
}
