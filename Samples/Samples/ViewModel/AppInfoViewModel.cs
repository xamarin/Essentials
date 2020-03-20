using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class AppInfoViewModel : BaseViewModel
    {
        public string AppPackageName => AppInfo.PackageName;

        public string AppName => AppInfo.Name;

        public string AppVersion => AppInfo.VersionString;

        public string AppBuild => AppInfo.BuildString;

        public string AppTheme => AppInfo.RequestedTheme.ToString();

        public string AppThemeUpdated { get; set; }

        public Command ShowSettingsUICommand { get; }

        public AppInfoViewModel()
        {
            ShowSettingsUICommand = new Command(() => AppInfo.ShowSettingsUI());
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            AppInfo.RequestedThemeChanged += AppInfoRequestedThemeChanged;
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            AppInfo.RequestedThemeChanged -= AppInfoRequestedThemeChanged;
        }

        void AppInfoRequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            AppThemeUpdated = $"{DateTime.UtcNow.ToShortTimeString()}: {e.RequestedTheme} updated";
            OnPropertyChanged(nameof(AppTheme));
            OnPropertyChanged(nameof(AppThemeUpdated));
        }
    }
}
