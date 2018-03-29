using System;
using System.Reflection;
using System.Web;
using UnitTests.HeadlessRunner;
using Windows.ApplicationModel.Activation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xunit.Runners.UI;

namespace Caboodle.DeviceTests.UWP
{
    public sealed partial class App : RunnerApplication
    {
        protected override async void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            if (args.Kind == ActivationKind.Protocol)
            {
                var protocolArgs = (ProtocolActivatedEventArgs)args;
                if (!string.IsNullOrEmpty(protocolArgs?.Uri?.Host))
                {
                    var parts = protocolArgs.Uri.Host.Split('_');
                    if (parts.Length >= 2 && !string.IsNullOrEmpty(parts[0]))
                    {
                        if (int.TryParse(parts[1], out var port))
                        {
#pragma warning disable 4014
                            try
                            {
                                Tests.RunAsync(parts[0], port, Traits.GetCommonTraits(), typeof(Battery_Tests).Assembly);
                            }
                            catch (Exception ex)
                            {
                                var m = new MessageDialog("Ex: " + ex.ToString());
                                await m.ShowAsync();
                            }
#pragma warning restore 4014
                        }
                    }
                }
            }

            var rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        protected override void OnInitializeRunner()
        {
            AddTestAssembly(typeof(App).GetTypeInfo().Assembly);
        }
    }
}
