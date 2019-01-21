using System;
using System.IO;
using System.Threading.Tasks;
using AVFoundation;
using CoreTelephony;
using Foundation;
using UIKit;

namespace Xamarin.Essentials
{
    public static partial class Recorder
    {
        static bool CanRecordAudio => AVAudioSession.SharedInstance().InputAvailable;

        static AVAudioRecorder recorder;

        static void InitAudioSession()
        {
            var audioSession = AVAudioSession.SharedInstance();

            var err = audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);
            if (err != null)
                throw new Exception(err.ToString());

            err = audioSession.SetActive(true);
            if (err != null)
                throw new Exception(err.ToString());
        }

        static void InitAudioRecorder()
        {
            var url = NSUrl.FromFilename(GetTempFileName());
            recorder = AVAudioRecorder.Create(url, new AudioSettings(settings), out var error);
            if (error != null)
                throw new Exception(error.ToString());
        }

        static string GetTempFileName()
        {
            var docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libFolder = Path.Combine(docFolder, "..", "Library");
            var tempFileName = Path.Combine(libFolder, Path.GetTempFileName());

            return tempFileName;
        }

        static Task PlatformRecordAsync()
        {
            InitAudioSession();
            InitAudioRecorder();
            recorder.Record();
            return Task.CompletedTask;
        }

        public Task<AudioRecording> StopAsync()
        {
            recorder.Stop();
            recorder.Dispose();
            recorder = null;
            var audioSession = AVAudioSession.SharedInstance();
            audioSession.SetActive(false);
            var recording = new AudioRecording(recorder.Url.Path);

            return Task.FromResult(recording);
        }

        private static NSDictionary<NSString, NSObject> settings = new NSDictionary<NSString, NSObject>(
        new[]
        {
            AVAudioSettings.AVSampleRateKey,
            AVAudioSettings.AVFormatIDKey,
            AVAudioSettings.AVNumberOfChannelsKey,
            AVAudioSettings.AVLinearPCMBitDepthKey,
            AVAudioSettings.AVLinearPCMIsBigEndianKey,
            AVAudioSettings.AVLinearPCMIsFloatKey
#pragma warning disable SA1118 // Parameter should not span multiple lines
        }, new NSObject[]
        {
            NSNumber.FromFloat(16000),
            NSNumber.FromInt32((int)AudioToolbox.AudioFormatType.LinearPCM),
            NSNumber.FromInt32(1),
            NSNumber.FromInt32(16),
            NSNumber.FromBoolean(false),
            NSNumber.FromBoolean(false)
        });
#pragma warning restore SA1118 // Parameter should not span multiple lines
    }
}
