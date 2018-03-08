using System.Windows.Input;
using Microsoft.Caboodle;
using Xamarin.Forms;

namespace Caboodle.Samples.ViewModel
{
    public class PhoneDialerViewModel : BaseViewModel
    {
        string phoneNumber;

        public PhoneDialerViewModel()
        {
            OpenPhoneDialerCommand = new Command(OnOpenPhoneDialer);
        }

        public ICommand OpenPhoneDialerCommand { get; }

        public bool IsEnabled => PhoneDialer.IsSupported && !string.IsNullOrEmpty(PhoneNumber);

        public string IsSupportedMessage => $"Is supported? {PhoneDialer.IsSupported}";

        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                if (SetProperty(ref phoneNumber, value))
                {
                    OnPropertyChanged(nameof(IsEnabled));
                }
            }
        }

        void OnOpenPhoneDialer() => PhoneDialer.Open(PhoneNumber);
    }
}
