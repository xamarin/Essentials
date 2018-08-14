using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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

        public Command OpenSettingsCommand { get; }

        public IList<AppStateChange> History { get; } = new ObservableCollection<AppStateChange>();

        public AppInfoViewModel()
        {
            OpenSettingsCommand = new Command(() => AppInfo.OpenSettings());
            AppInfo.AppStateChanged += UpdateState;
        }

        public override void OnDisappearing()
        {
            Task.Run(async () =>
                {
                    await Task.Delay(1500); // To catch the event if this is to test app state
                    AppInfo.AppStateChanged -= UpdateState;
                });
        }

        void UpdateState(object sender, AppStateChangedEventArgs args)
        {
            History.Add(new AppStateChange(args.State, DateTimeOffset.UtcNow));
        }
    }

    public readonly struct AppStateChange
    {
        public AppStateChange(AppState state, DateTimeOffset changeTime)
        {
            AppState = state.ToString();
            ChangeTime = changeTime.ToString("yyyy.MM.dd - HH:mm");
        }

        public string AppState { get; }

        public string ChangeTime { get; }
    }
}
