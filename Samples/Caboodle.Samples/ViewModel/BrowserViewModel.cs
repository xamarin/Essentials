using System;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Caboodle;
using Xamarin.Forms;

namespace Caboodle.Samples.ViewModel
{
    public class BrowserViewModel : BaseViewModel
    {
        string browserstatus;

        public ICommand OpenUriCommand { get; }

        public string BrowserStatus
        {
            get => browserstatus;
            set => SetProperty(ref browserstatus, value);
        }

        public BrowserViewModel()
        {
            OpenUriCommand = new Command(async () =>
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                try
                {
                    await Browser.OpenAsync(uri, launchType);
                }
                catch (Exception e)
                {
                    browserstatus = $"Unable to open Uri {e.Message}";
                    System.Diagnostics.Debug.WriteLine(browserstatus);
                }
                finally
                {
                    IsBusy = false;
                }
            });

            return;
        }

        string uri = "http://xamarin.com";

        public string Uri
        {
            get => uri;
            set
            {
                uri = value;

                OnPropertyChanged();
            }
        }

        List<string> browserlaunchertypes = new List<string>
        {
            $"Uri Launcher",
            $"System Browser(CustomTabs, Safari)",
        };

        public List<string> BrowserLaunchTypes
        {
            get => browserlaunchertypes;
            set
            {
                browserlaunchertypes = value;
                OnPropertyChanged();
            }
        }

        BrowserLaunchingType launchType = BrowserLaunchingType.SystemBrowser;

        string browsertype = $"System Browser(CustomTabs, Safari)";

        public string BrowserType
        {
            get => browsertype;
            set
            {
                browsertype = value;
                if (browsertype == "Uri Launcher")
                {
                    launchType = BrowserLaunchingType.UriLauncher;
                }
                else
                {
                    launchType = BrowserLaunchingType.SystemBrowser;
                }

                OnPropertyChanged();
            }
        }
    }
}
