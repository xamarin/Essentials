using System;
using System.IO;
using System.Reflection;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Xunit.Runners.UI;

namespace Caboodle.DeviceTests.UWP
{
    public sealed partial class App : RunnerApplication
    {
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            base.OnLaunched(e);

            // Invoke the headless test runner if a config was specified
            var testsCfgFile = StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///tests.cfg")).GetResults();
            var testCfg = File.ReadAllText(testsCfgFile.Path)?.Split(':');
            if (testCfg != null && testCfg.Length > 1)
            {
                var ip = testCfg[0];
                int port;
                if (int.TryParse(testCfg[1], out port))
                    UnitTests.HeadlessRunner.Tests.RunAsync(ip, port, typeof(Battery_Tests).Assembly);
            }
        }

        protected override void OnInitializeRunner()
        {
            AddTestAssembly(typeof(App).GetTypeInfo().Assembly);
        }
    }
}
