using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class DataTransferViewModel : BaseViewModel
    {
        private bool shareText = true;
        private bool shareUri;
        private string text;
        private string uri;
        private string subject;
        private string title;

        public ICommand RequestCommand { get; }

        public DataTransferViewModel()
        {
            RequestCommand = new Command(OnRequest);
        }

        public bool ShareText
        {
            get => shareText;
            set => SetProperty(ref shareText, value);
        }

        public bool ShareUri
        {
            get => shareUri;
            set => SetProperty(ref shareUri, value);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Uri
        {
            get => uri;
            set => SetProperty(ref uri, value);
        }

        public string Subject
        {
            get => subject;
            set => SetProperty(ref subject, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private async void OnRequest()
        {
            await DataTransfer.RequestAsync(new ShareTextRequest
            {
                Subject = Subject,
                Text = ShareText ? Text : null,
                Uri = ShareUri ? Uri : null,
                Title = Title
            });
        }
    }
}
