using System;
using System.Threading.Tasks;
using Android.Content;

namespace Xamarin.Essentials
{
    public static partial class Clipboard
    {
        static Lazy<ClipboardChangeListener> ClipboardListener
            = new Lazy<ClipboardChangeListener>(() => new ClipboardChangeListener());
        
        static Task PlatformSetTextAsync(string text)
        {
            Platform.ClipboardManager.PrimaryClip = ClipData.NewPlainText("Text", text);
            return Task.CompletedTask;
        }

        static bool PlatformHasText
            => Platform.ClipboardManager.HasPrimaryClip;

        static Task<string> PlatformGetTextAsync()
            => Task.FromResult(Platform.ClipboardManager.PrimaryClip?.GetItemAt(0)?.Text);

        static void StartClipboardListeners()
Platform.ClipboardManager.PrimaryClip.AddPrimaryClipChangedListener(ClipboardListener.Value);

        static void StopClipboardListeners()
            => Platform.ClipboardManager.PrimaryClip.RemovePrimaryClipChangedListener(ClipboardListener.Value);
    }


    public class ClipboardChangeListener : IOnPrimaryClipChangedListener 
    {
        override void OnPrimaryClipChanged()
        {
            Clipboard.ClipboardChangedInternal();
        }
    }
}
