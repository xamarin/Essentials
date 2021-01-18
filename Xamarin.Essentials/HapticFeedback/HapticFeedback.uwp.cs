using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Haptics;

namespace Xamarin.Essentials
{
    public static partial class HapticFeedback
    {
        const string vibrationDeviceApiType = "Windows.Devices.Haptics.VibrationDevice";

        static async void PlatformPerform(HapticFeedbackType type)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent(vibrationDeviceApiType)
                && await VibrationDevice.RequestAccessAsync() == VibrationAccessStatus.Allowed)
            {
                var controller = (await VibrationDevice.GetDefaultAsync())?.SimpleHapticsController;

                if (controller != null)
                {
                    var feedback = FindFeedback(controller, ConvertType(type));
                    if (feedback != null)
                        controller.SendHapticFeedback(feedback);
                }
            }
        }

        static HapticFeedbackGenerator PlatformPrepareGenerator(HapticFeedbackType type)
            => new HapticFeedbackGenerator(() => PlatformPerform(type));

        static SimpleHapticsControllerFeedback FindFeedback(SimpleHapticsController controller, ushort type)
        {
            foreach (var feedback in controller.SupportedFeedback)
            {
                if (feedback.Waveform == type)
                    return feedback;
            }
            return null;
        }

        static ushort ConvertType(HapticFeedbackType type) =>
            type switch
            {
                HapticFeedbackType.LongPress => KnownSimpleHapticsControllerWaveforms.Press,
                _ => KnownSimpleHapticsControllerWaveforms.Click
            };
    }

    public partial class HapticFeedbackGenerator : IDisposable
    {
        Action perform;

        internal HapticFeedbackGenerator(Action perform)
            => this.perform = perform;

        void PlatformPerform()
            => perform.Invoke();

        void PlatformDispose()
            => perform = null;
    }
}
