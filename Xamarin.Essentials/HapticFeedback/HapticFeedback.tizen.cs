﻿using System;
using System.Diagnostics;
using Tizen.System;

namespace Xamarin.Essentials
{
    public static partial class HapticFeedback
    {
        static void PlatformPerform(HapticFeedbackType type)
        {
            Permissions.EnsureDeclared<Permissions.Vibrate>();
            try
            {
                var feedback = new Feedback();
                var pattern = ConvertType(type);
                if (feedback.IsSupportedPattern(FeedbackType.Vibration, pattern))
                    feedback.Play(FeedbackType.Vibration, pattern);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HapticFeedback Exception: {ex.Message}");
            }
        }

        public static HapticFeedbackGenerator PlatformGetGenerator(HapticFeedbackType type = HapticFeedbackType.Click)
            => new HapticFeedbackGenerator(() => PlatformPerform(type));

        static string ConvertType(HapticFeedbackType type) =>
            type switch
            {
                HapticFeedbackType.LongPress => "Hold",
                _ => "Tap"
            };
    }

    public partial class HapticFeedbackGenerator
    {
        Action perform;

        internal HapticFeedbackGenerator(Action perform)
            => this.perform = perform;

        public virtual void PlatformPerform()
            => perform.Invoke();

        public virtual void PlatformDispose()
            => perform = null;
    }
}
