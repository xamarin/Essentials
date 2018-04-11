using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class ClipboardViewModel : BaseViewModel
    {
        private string fieldValue;

        public ClipboardViewModel()
        {
            CopyCommand = new Command(OnCopy);
            PasteCommand = new Command(OnPaste);
        }

        public ICommand CopyCommand { get; }

        public ICommand PasteCommand { get; }

        public string FieldValue
        {
            get => fieldValue;
            set => SetProperty(ref fieldValue, value);
        }

        private void OnCopy() => Clipboard.SetText(FieldValue);

        private async void OnPaste()
        {
            var text = await Clipboard.GetTextAsync();
            if (!string.IsNullOrWhiteSpace(text))
            {
                FieldValue = text;
            }
        }
    }
}
