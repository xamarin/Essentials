using System.Windows.Input;
using Microsoft.Caboodle;
using Xamarin.Forms;

namespace Caboodle.Samples.ViewModel
{
    public class WakeLockViewModel : BaseViewModel
    {
        public WakeLockViewModel()
        {
            RequestActiveCommand = new Command(OnRequestActive);
            RequestReleaseCommand = new Command(OnRequestRelease);
        }

        public ICommand RequestActiveCommand { get; }

        public ICommand RequestReleaseCommand { get; }

        void OnRequestActive()
        {
            WakeLock.RequestActive();

            OnPropertyChanged(nameof(IsActive));
        }

        void OnRequestRelease()
        {
            WakeLock.RequestRelease();

            OnPropertyChanged(nameof(IsActive));
        }

        public bool IsActive => WakeLock.IsActive;
    }
}
