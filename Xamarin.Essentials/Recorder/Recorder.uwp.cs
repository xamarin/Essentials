using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;

namespace Xamarin.Essentials
{
    public static partial class Recorder
    {
        internal static bool PlatformCanRecord => true;

        static MediaCapture mediaCapture;

        static string audioFilePath;

        static async Task PlatformRecordAsync()
        {
            try
            {
                mediaCapture = new MediaCapture();
                await mediaCapture.InitializeAsync(
                    new MediaCaptureInitializationSettings { StreamingCaptureMode = StreamingCaptureMode.Audio });
                mediaCapture.RecordLimitationExceeded += sender =>
                {
                    DeleteMediaCapture();
                    throw new Exception("Record Limitation Exceeded");
                };

                mediaCapture.Failed += (sender, errorEventArgs) =>
                {
                    DeleteMediaCapture();
                    throw new Exception($"Audio recording failed: {errorEventArgs.Code}. {errorEventArgs.Message}");
                };

                var fileOnDisk = await ApplicationData.Current.LocalFolder.CreateFileAsync(Path.GetRandomFileName());
                audioFilePath = fileOnDisk.Path;
                await mediaCapture.StartRecordToStorageFileAsync(MediaEncodingProfile.CreateWav(AudioEncodingQuality.Auto), fileOnDisk);
            }
            catch
            {
                DeleteMediaCapture();
                throw;
            }
        }

        internal static async Task<AudioRecording> StopAsync()
        {
            if (mediaCapture == null)
                throw new InvalidOperationException("No recording in progress");

            await mediaCapture.StopRecordAsync();

            mediaCapture.Dispose();
            mediaCapture = null;

            return GetRecording();
        }

        static void DeleteMediaCapture()
        {
            try
            {
                mediaCapture?.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error disposing audio capture: {ex}");
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(audioFilePath) && File.Exists(audioFilePath))
                    File.Delete(audioFilePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting audio file: {ex}");
            }

            audioFilePath = string.Empty;
            mediaCapture = null;
        }
    }
}
