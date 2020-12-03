using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class MediaPickerViewModel : BaseViewModel
    {
        string photoPath;
        string videoPath;

        bool showPhoto;
        bool showVideo;

        bool usePopover;

        public MediaPickerViewModel()
        {
            PickPhotoCommand = new Command<Xamarin.Forms.View>(DoPickPhoto);
            CapturePhotoCommand = new Command<Xamarin.Forms.View>(DoCapturePhoto, _ => MediaPicker.IsCaptureSupported);

            PickVideoCommand = new Command<Xamarin.Forms.View>(DoPickVideo);
            CaptureVideoCommand = new Command<Xamarin.Forms.View>(DoCaptureVideo, _ => MediaPicker.IsCaptureSupported);
        }

        public ICommand PickPhotoCommand { get; }

        public ICommand CapturePhotoCommand { get; }

        public ICommand PickVideoCommand { get; }

        public ICommand CaptureVideoCommand { get; }

        public bool ShowPhoto
        {
            get => showPhoto;
            set => SetProperty(ref showPhoto, value);
        }

        public bool ShowVideo
        {
            get => showVideo;
            set => SetProperty(ref showVideo, value);
        }

        public string PhotoPath
        {
            get => photoPath;
            set => SetProperty(ref photoPath, value);
        }

        public string VideoPath
        {
            get => videoPath;
            set => SetProperty(ref videoPath, value);
        }

        public bool UsePopover
        {
            get => usePopover;
            set => SetProperty(ref usePopover, value);
        }

        async void DoPickPhoto(Xamarin.Forms.View view)
        {
            try
            {
                var options = UsePopover
                    ? new MediaPickerOptions { PresentationSourceBounds = GetRectangle(view), }
                    : null;

                var photo = await MediaPicker.PickPhotoAsync(options);

                await LoadPhotoAsync(photo);

                Console.WriteLine($"PickPhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PickPhotoAsync THREW: {ex.Message}");
            }
        }

        async void DoCapturePhoto(Xamarin.Forms.View view)
        {
            try
            {
                var options = UsePopover
                    ? new MediaPickerOptions { PresentationSourceBounds = GetRectangle(view), }
                    : null;

                var photo = await MediaPicker.CapturePhotoAsync(options);

                await LoadPhotoAsync(photo);

                Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        async void DoPickVideo(Xamarin.Forms.View view)
        {
            try
            {
                var options = UsePopover
                    ? new MediaPickerOptions { PresentationSourceBounds = GetRectangle(view), }
                    : null;

                var video = await MediaPicker.PickVideoAsync(options);

                await LoadVideoAsync(video);

                Console.WriteLine($"PickVideoAsync COMPLETED: {PhotoPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PickVideoAsync THREW: {ex.Message}");
            }
        }

        async void DoCaptureVideo(Xamarin.Forms.View view)
        {
            try
            {
                var options = UsePopover
                    ? new MediaPickerOptions { PresentationSourceBounds = GetRectangle(view), }
                    : null;

                var photo = await MediaPicker.CaptureVideoAsync(options);

                await LoadVideoAsync(photo);

                Console.WriteLine($"CaptureVideoAsync COMPLETED: {PhotoPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CaptureVideoAsync THREW: {ex.Message}");
            }
        }

        async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }

            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
            {
                await stream.CopyToAsync(newStream);
            }

            PhotoPath = newFile;
            ShowVideo = false;
            ShowPhoto = true;
        }

        async Task LoadVideoAsync(FileResult video)
        {
            // canceled
            if (video == null)
            {
                VideoPath = null;
                return;
            }

            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, video.FileName);
            using (var stream = await video.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
            {
                await stream.CopyToAsync(newStream);
            }

            VideoPath = newFile;
            ShowVideo = true;
            ShowPhoto = false;
        }

        public override void OnDisappearing()
        {
            PhotoPath = null;
            VideoPath = null;

            base.OnDisappearing();
        }
    }
}
