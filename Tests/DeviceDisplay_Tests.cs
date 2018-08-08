using Xamarin.Essentials;
using Xunit;

namespace Tests
{
    public class DeviceDisplay_Tests
    {
        [Fact]
        public void DeviceDisplay_Comparison_Equal()
        {
            var device1 = new ScreenMetrics
            {
                Width = 0,
                Height = 0,
                Density = 0,
                Orientation = ScreenOrientation.Landscape,
                Rotation = ScreenRotation.Rotation0
            };

            var device2 = new ScreenMetrics
            {
                Width = 0,
                Height = 0,
                Density = 0,
                Orientation = ScreenOrientation.Landscape,
                Rotation = ScreenRotation.Rotation0
            };

            Assert.True(device1.Equals(device2));
        }

        [Fact]
        public void DeviceDisplay_Comparison_NotEqual()
        {
            var device1 = new ScreenMetrics
            {
                Width = 0,
                Height = 0,
                Density = 0,
                Orientation = ScreenOrientation.Landscape,
                Rotation = ScreenRotation.Rotation0
            };

            var device2 = new ScreenMetrics
            {
                Width = 1,
                Height = 0,
                Density = 0,
                Orientation = ScreenOrientation.Landscape,
                Rotation = ScreenRotation.Rotation0
            };

            Assert.False(device1.Equals(device2));

            var device3 = new ScreenMetrics
            {
                Width = 0,
                Height = 1,
                Density = 0,
                Orientation = ScreenOrientation.Landscape,
                Rotation = ScreenRotation.Rotation0
            };

            Assert.False(device1.Equals(device3));

            var device4 = new ScreenMetrics
            {
                Width = 0,
                Height = 0,
                Density = 1,
                Orientation = ScreenOrientation.Landscape,
                Rotation = ScreenRotation.Rotation0
            };

            Assert.False(device1.Equals(device4));

            var device5 = new ScreenMetrics
            {
                Width = 0,
                Height = 0,
                Density = 0,
                Orientation = ScreenOrientation.Portrait,
                Rotation = ScreenRotation.Rotation0
            };

            Assert.False(device1.Equals(device5));

            var device6 = new ScreenMetrics
            {
                Width = 0,
                Height = 0,
                Density = 0,
                Orientation = ScreenOrientation.Landscape,
                Rotation = ScreenRotation.Rotation180
            };

            Assert.False(device1.Equals(device6));
        }
    }
}
