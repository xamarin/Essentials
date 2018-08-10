using Xamarin.Essentials;
using Xunit;

namespace Tests
{
    public class DeviceDisplay_Tests
    {
        [Fact]
        public void DeviceDisplay_Comparison_Equal()
        {
            var device1 = new ScreenMetrics(
                width: 0,
                height: 0,
                density: 0,
                orientation: ScreenOrientation.Landscape,
                rotation: ScreenRotation.Rotation0);

            var device2 = new ScreenMetrics(
                width: 0,
                height: 0,
                density: 0,
                orientation: ScreenOrientation.Landscape,
                rotation: ScreenRotation.Rotation0);

            Assert.True(device1.Equals(device2));
            Assert.True(device1 == device2);
            Assert.False(device1 != device2);
        }

        [Fact]
        public void DeviceDisplay_Comparison_NotEqual()
        {
            var device1 = new ScreenMetrics(
                width: 0,
                height: 0,
                density: 0,
                orientation: ScreenOrientation.Landscape,
                rotation: ScreenRotation.Rotation0);

            var device2 = new ScreenMetrics(
                width: 1,
                height: 0,
                density: 0,
                orientation: ScreenOrientation.Landscape,
                rotation: ScreenRotation.Rotation0);

            Assert.False(device1.Equals(device2));
            Assert.True(device1 != device2);
            Assert.False(device1 == device2);

            var device3 = new ScreenMetrics(
                width: 0,
                height: 1,
                density: 0,
                orientation: ScreenOrientation.Landscape,
                rotation: ScreenRotation.Rotation0);

            Assert.False(device1.Equals(device3));
            Assert.True(device1 != device3);
            Assert.False(device1 == device3);

            var device4 = new ScreenMetrics(
                width: 0,
                height: 0,
                density: 1,
                orientation: ScreenOrientation.Landscape,
                rotation: ScreenRotation.Rotation0);

            Assert.False(device1.Equals(device4));
            Assert.True(device1 != device4);
            Assert.False(device1 == device4);

            var device5 = new ScreenMetrics(
                width: 0,
                height: 0,
                density: 0,
                orientation: ScreenOrientation.Portrait,
                rotation: ScreenRotation.Rotation0);

            Assert.False(device1.Equals(device5));
            Assert.True(device1 != device5);
            Assert.False(device1 == device5);

            var device6 = new ScreenMetrics(
                width: 0,
                height: 0,
                density: 0,
                orientation: ScreenOrientation.Landscape,
                rotation: ScreenRotation.Rotation90);

            Assert.False(device1.Equals(device6));
            Assert.True(device1 != device6);
            Assert.False(device1 == device6);
        }
    }
}
