using System.Threading;
using System.Windows.Input;
using Samples.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class TextToSpeechViewModel : BaseViewModel
    {
        public TextToSpeechViewModel()
        {
            SpeakCommand = new Command(OnSpeak);
            Text = "Xamarin Essentials makes text to speech easy!";

            AdvancedSettings = false;
            Volume = 1.0f;
            Pitch = 1.0f;
            Rate = 1.0f;

            return;
        }

        void OnSpeak(object obj)
        {
            var settings = new SpeakSettings()
            {
                Volume = Volume,
                Pitch = Pitch,
                SpeakRate = Rate,
            };

            if (AdvancedSettings)
            {
                TextToSpeech.SpeakAsync(Text, settings, default(CancellationToken));
            }
            else
            {
                TextToSpeech.SpeakAsync(Text, default(CancellationToken));
            }

            return;
        }

        public ICommand SpeakCommand { get; }

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
    }
}
