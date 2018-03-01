using System.IO;
using System.Windows.Input;
using MvvmHelpers;
using Xamarin.Forms;
using Microsoft.Caboodle;

namespace Caboodle.Samples.ViewModel
{
    public class FileSystemViewModel : BaseViewModel
    {
        const string TemplateFileName = "FileSystemTemplate.txt";
        const string LocalFileName = "TheFile.txt";

        static string LocalPath = Path.Combine(FileSystem.AppDataDirectory, LocalFileName);

        private string currentContents;

        public FileSystemViewModel()
        {
            LoadFileCommand = new Command(() => DoLoadFile());
            SaveFileCommand = new Command(() => DoSaveFile());
            DeleteFileCommand = new Command(() => DoDeleteFile());

            DoLoadFile();
        }

        public ICommand LoadFileCommand { get; }

        public ICommand SaveFileCommand { get; }

        public ICommand DeleteFileCommand { get; }

        public string CurrentContents
        {
            get => currentContents;
            set => SetProperty(ref currentContents, value);
        }

        private async void DoLoadFile()
        {
            if (File.Exists(LocalPath))
            {
                CurrentContents = File.ReadAllText(LocalPath);
            }
            else
            {
                using (var stream = await FileSystem.OpenAppPackageFileAsync(TemplateFileName))
                using (var reader = new StreamReader(stream))
                {
                    CurrentContents = await reader.ReadToEndAsync();
                }
            }
        }

        private void DoSaveFile()
        {
            File.WriteAllText(LocalPath, CurrentContents);
        }

        private void DoDeleteFile()
        {
            if (File.Exists(LocalPath))
                File.Delete(LocalPath);
        }
    }
}
