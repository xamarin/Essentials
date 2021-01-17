using System;

namespace Xamarin.Essentials
{
    public static partial class HapticFeedback
    {
        public static void Perform(HapticFeedbackType type = HapticFeedbackType.Click)
            => PlatformPerform(type);

        public static HapticFeedbackGenerator PrepareGenerator(HapticFeedbackType type = HapticFeedbackType.Click)
#if MONOANDROID || WINDOWS_UWP || __MACOS__ || TIZEN
            => new HapticFeedbackGenerator(() => PlatformPerform(type));

#else
            => PlatformPrepareGenerator(type);
#endif
    }

    public partial class HapticFeedbackGenerator : IDisposable
    {
#if MONOANDROID || WINDOWS_UWP || __MACOS__ || TIZEN
        Action perform;

        internal HapticFeedbackGenerator(Action perform)
            => this.perform = perform;

#endif
        protected internal HapticFeedbackGenerator()
        {
        }

        public void Perform()
#if MONOANDROID || WINDOWS_UWP || __MACOS__ || TIZEN
            => perform.Invoke();
#else
            => PlatformPerform();
#endif

        public void Dispose()
#if MONOANDROID || WINDOWS_UWP || __MACOS__ || TIZEN
            => perform = null;
#else
        {
            PlatformDispose();
            GC.SuppressFinalize(this);
        }
#endif
    }
}
