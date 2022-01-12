using System;
using UIKit;

namespace DeviceTests.iOS
{
    public class Application
    {
        static void Main(string[] args)
        {
            if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ci-run")))
                UIApplication.Main(args, null, typeof(TestApplicationDelegate));
            else
                UIApplication.Main(args, null, typeof(AppDelegate));
        }
    }
}
