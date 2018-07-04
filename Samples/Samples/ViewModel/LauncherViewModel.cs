using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class LauncherViewModel : BaseViewModel
    {
        public string LaunchUri { get; set; }

        public ICommand LaunchCommand { get; }

        public ICommand CanLaunchCommand { get; }

        public LauncherViewModel()
        {
            LaunchCommand = new Command(OnLaunch);
            CanLaunchCommand = new Command(CanLaunch);
        }

        async void OnLaunch()
        {
            await Launcher.OpenAsync(LaunchUri);
        }

        async void CanLaunch()
        {
            var canBeLaunched = await Launcher.CanOpenAsync(LaunchUri);
            await DisplayAlertAsync($"Uri {LaunchUri} can be Launched: {canBeLaunched}");
        }
    }
}
