using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppKit;

namespace Xamarin.Essentials
{
    public static partial class Contacts
    {
        static Task<Contact> PlatformPickContactAsync()
        {
            NSHapticFeedbackManager.DefaultPerformer.PerformFeedback(NSHapticFeedbackPattern.Generic, NSHapticFeedbackPerformanceTime.Default);
            return null;
        }

        static async Task<IEnumerable<Contact>> PlatformGetAllAsync()
        {
            await Task.CompletedTask;
            return null;
        }
    }
}
