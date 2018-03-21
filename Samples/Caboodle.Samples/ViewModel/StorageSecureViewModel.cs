using Microsoft.Caboodle;

namespace Caboodle.Samples.ViewModel
{
    public class StorageSecureViewModel : BaseViewModel
    {
        readonly SecureStorage storagesecure;

        public StorageSecureViewModel()
        {
            storagesecure = new SecureStorage();

            return;
        }

        private string filename;

        public string FileName
        {
            get => filename;
            set
            {
                filename = value;
                OnPropertyChanged();
            }
        }

        private string storedtext;

        public string StoredText
        {
            get => storedtext;
            set
            {
                storedtext = value;
                OnPropertyChanged();
            }
        }
    }
}
