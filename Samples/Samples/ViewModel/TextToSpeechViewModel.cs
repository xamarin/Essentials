using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Samples.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class TextToSpeechViewModel : BaseViewModel
    {
        CancellationTokenSource cts;

        public TextToSpeechViewModel()
        {
            SpeakCommand = new Command<bool>(OnSpeak);
            CancelCommand = new Command(OnCancel);
            PickLocaleCommand = new Command(async () => await OnPickLocale());

            Text = "Xamarin Essentials makes text to speech easy!";

            AdvancedSettings = false;
            Volume = 1.0f;
            Pitch = 1.0f;
            Rate = 1.0f;
        }

        void OnSpeak(bool multiple)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            cts = new CancellationTokenSource();

            var settings = AdvancedSettings ?
            new SpeakSettings()
            {
                Volume = Volume,
                Pitch = Pitch,
                SpeakRate = Rate,
                Locale = selectedLocale
            }
            : null;

            if (multiple)
            {
                Task.Run(async () =>
                {
                    await TextToSpeech.SpeakAsync(Text + " 1 ", settings, cancelToken: cts.Token);
                    await TextToSpeech.SpeakAsync(Text + " 2 ", settings, cts.Token);
                    await TextToSpeech.SpeakAsync(Text + " 3 ", settings, cts.Token);
                    IsBusy = false;
                });
            }
            else
            {
                TextToSpeech.SpeakAsync(Text, settings, cts.Token).ContinueWith((t) => { IsBusy = false; });
            }
        }

        void OnCancel()
        {
            if (!IsBusy && !cts.IsCancellationRequested)
                return;

            cts.Cancel();

            IsBusy = false;
        }

        Locale selectedLocale;

        async Task OnPickLocale()
        {
            var items = await TextToSpeech.GetLocalesAsync();
            var result = await Application.Current.MainPage.DisplayActionSheet("Pick", "OK", null, items.Select(i => i.Name).ToArray());
            selectedLocale = items.FirstOrDefault(i => i.Name == result);
            Locale = (result == "OK" || string.IsNullOrEmpty(result)) ? "Default" : result;
        }

        public ICommand CancelCommand { get; }

        public ICommand SpeakCommand { get; }

        public ICommand PickLocaleCommand { get; }

        string text;

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        bool advancedSettings;

        public bool AdvancedSettings
        {
            get => advancedSettings;
            set => SetProperty(ref advancedSettings, value);
        }

        float volume;

        public float Volume
        {
            get => volume;
            set => SetProperty(ref volume, value);
        }

        float pitch;

        public float Pitch
        {
            get => pitch;
            set => SetProperty(ref pitch, value);
        }

        float rate;

        public float Rate
        {
            get => rate;
            set => SetProperty(ref rate, value);
        }

        string locale = "Default";

        public string Locale
        {
            get => locale;
            set => SetProperty(ref locale, value);
        }
    }
}
