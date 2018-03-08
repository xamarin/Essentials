using System;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Caboodle;
using Xamarin.Forms;

namespace Caboodle.Samples.ViewModel
{
    public class BrowserViewModel : BaseViewModel
    {
        private Browser browser;

        private string browserstatus;

        public ICommand OpenUriCommand { get; }

        public string BrowserStatus
        {
            get => browserstatus;
            set => SetProperty(ref browserstatus, value);
        }

        public BrowserViewModel()
        {
            BrowserType = BrowserLaunchTypes[0];
            browser = new Browser();

            OpenUriCommand = new Command(async () =>
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                try
                {
                    await Browser.OpenAsync(uri);
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

        string uri;

        public string Uri
        {
            get => uri;
            set
            {
                uri = value;

                OnPropertyChanged();
            }
        }

        private List<string> browserlaunchertypes = new List<string>
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

        private string browsertype;

        public string BrowserType
        {
            get => browsertype;
            set
            {
                browsertype = value;
                if (browsertype == "Uri Launcher")
                {
                    Browser.AlwaysUseExternal = false;
                }
                else
                {
                    Browser.AlwaysUseExternal = true;
                }

                OnPropertyChanged();
            }
        }

        private List<string> uris = new List<string>
        {
            $"https://xamarin.com",
        };
    }
}
