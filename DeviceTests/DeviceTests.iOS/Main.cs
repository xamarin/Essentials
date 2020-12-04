using System;
using System.Threading.Tasks;
using UIKit;
using Xunit;

namespace DeviceTests.iOS
{
    public class Application
    {
        static void Main(string[] args)
        {
            // UIApplication.Main(args, null, nameof(AppDelegate));

            TestInstrumentation.StartAsync(args).Wait();

            Console.WriteLine("----- Done -----");
        }
    }

    public class MyTEsts
    {
        [Fact]
        public void TheTest()
        {
            Assert.True(true);
        }
    }
}
