using Xamarin.Essentials;

namespace Samples.ViewModel
{
    public class PreferencesViewModel : BaseViewModel
    {
        private const string preferenceKey = "PreferenceKey";

        private string preferenceValue;

        public PreferencesViewModel()
        {
            preferenceValue = Preferences.Get(preferenceKey, string.Empty);
        }

        public string PreferenceValue
        {
            get => preferenceValue;
            set
            {
                preferenceValue = value;
                Preferences.Set(preferenceKey, value);

                OnPropertyChanged();
            }
        }
    }
}
