using System;
using Tizen.System;

namespace Xamarin.Essentials
{
    public static partial class HapticFeedback
    {
        static void PlatformPerform(HapticFeedbackType type)
        {
            using var generator = PlatformPrepareGenerator(type);
            generator?.Perform();
        }

        static HapticFeedbackGenerator PlatformPrepareGenerator(HapticFeedbackType type)
            => new HapticFeedbackGenerator(type).Init();
    }

    public partial class HapticFeedbackGenerator : IDisposable
    {
        string pattern;
        Feedback feedback;

        internal HapticFeedbackGenerator Init()
        {
            Permissions.EnsureDeclared<Permissions.Vibrate>();
            pattern = ConvertType(Type);
            feedback = new Feedback();

            if (!feedback.IsSupportedPattern(FeedbackType.Vibration, pattern))
               throw new FeatureNotSupportedException(HapticFeedback.notSupportedMessage);

            return this;
        }

        void PlatformPerform()
            => feedback.Play(FeedbackType.Vibration, pattern);

        void PlatformDispose()
        {
            pattern = null;
            feedback = null;
        }

        string ConvertType(HapticFeedbackType type)
            => type switch
            {
                HapticFeedbackType.LongPress => "Hold",
                HapticFeedbackType.Click => throw new NotImplementedException(),
                _ => "Tap"
            };
    }
}
