﻿using UIKit;

namespace DeviceTests.iOS
{
    public class Application
    {
        static void Main(string[] args)
        {
            if (args?.Length > 0) // usually means this is from xharness
                UIApplication.Main(args, null, typeof(TestApplicationDelegate));
            else
                UIApplication.Main(args, null, typeof(AppDelegate));
        }
    }
}
