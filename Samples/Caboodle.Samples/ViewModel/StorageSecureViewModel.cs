using Microsoft.Caboodle;

namespace Caboodle.Samples.ViewModel
{
    public class StorageSecureViewModel : BaseViewModel
    {
        string filename;

        public string FileName
        {
            get => filename;
            set => base.SetProperty(ref filename, value);
        }

        string storedtext;

        public string StoredText
        {
            get => storedtext;
            set => base.SetProperty(ref storedtext, value);
        }
    }
}
