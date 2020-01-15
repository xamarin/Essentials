using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class WebAuthenticatorViewModel : BaseViewModel
    {
        public WebAuthenticatorViewModel()
        {
            AuthenticateCommand = new Command(async () => await OnAuthenticate());
        }

        public ICommand AuthenticateCommand { get; }

        string accessToken = string.Empty;

        public string AccessToken
        {
            get => accessToken;
            set => SetProperty(ref accessToken, value);
        }

        async Task OnAuthenticate()
        {
            try
            {
                var authUrl = new Uri("https://redirect.me/?xamarinessentials%3A%2F%2F%3Faccess_token%3D123XYZ-TOKEN");
                var callbackUrl = new Uri("xamarinessentials://");

                var r = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);

                AccessToken = r?.AccessToken;
            }
            catch (Exception ex)
            {
                AccessToken = string.Empty;
                await DisplayAlertAsync($"Failed: {ex.Message}");
            }
        }
    }
}
