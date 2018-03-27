using System;
using System.Threading.Tasks;
using Microsoft.Caboodle;
using Xunit;

namespace Caboodle.DeviceTests
{
    public class Compass_Tests
    {
        public Compass_Tests()
        {
            Compass.StopMonitor();
        }

        [Fact]
        [Trait(Traits.DeviceType, Traits.DeviceTypes.Physical)]
        [Trait(Traits.DeviceType, Traits.DeviceTypes.Virtual)]
        public void IsSupported()
        {
            if (DeviceInfo.DeviceType == DeviceType.Virtual && DeviceInfo.Platform == DeviceInfo.Platforms.iOS)
            {
                Assert.False(Compass.IsSupported);
                return;
            }

            Assert.True(Compass.IsSupported);
        }

        [Fact]
        [Trait(Traits.DeviceType, Traits.DeviceTypes.Physical)]
        [Trait(Traits.DeviceType, Traits.DeviceTypes.Virtual)]
        public void Monitor_Null_Handler()
        {
            if (DeviceInfo.DeviceType == DeviceType.Virtual && DeviceInfo.Platform == DeviceInfo.Platforms.iOS)
            {
                return;
            }

            Assert.Throws<ArgumentNullException>(() => Compass.Monitor(SensorSpeed.Normal, null));
        }

        [Theory]
        [InlineData(SensorSpeed.Fastest)]
        [Trait(Traits.DeviceType, Traits.DeviceTypes.Physical)]
        [Trait(Traits.DeviceType, Traits.DeviceTypes.Virtual)]
        public async Task Monitor(SensorSpeed sensorSpeed)
        {
            if (DeviceInfo.DeviceType == DeviceType.Virtual && DeviceInfo.Platform == DeviceInfo.Platforms.iOS)
            {
                return;
            }

            var tcs = new TaskCompletionSource<CompassData>();
            Compass.Monitor(sensorSpeed, (data) =>
            {
                tcs.TrySetResult(data);
            });

            var d = await tcs.Task;

            Assert.True(d.HeadingMagneticNorth >= 0);
        }

        [Theory]
        [InlineData(SensorSpeed.Fastest)]
        [Trait(Traits.DeviceType, Traits.DeviceTypes.Physical)]
        [Trait(Traits.DeviceType, Traits.DeviceTypes.Virtual)]
        public async Task IsMonitoring(SensorSpeed sensorSpeed)
        {
            if (DeviceInfo.DeviceType == DeviceType.Virtual && DeviceInfo.Platform == DeviceInfo.Platforms.iOS)
            {
                return;
            }

            var tcs = new TaskCompletionSource<CompassData>();
            Compass.Monitor(sensorSpeed, (data) =>
            {
                tcs.TrySetResult(data);
            });

            var d = await tcs.Task;
            Assert.True(Compass.IsMonitoring);
            Compass.StopMonitor();
        }

        [Theory]
        [InlineData(SensorSpeed.Fastest)]
        [Trait(Traits.DeviceType, Traits.DeviceTypes.Physical)]
        [Trait(Traits.DeviceType, Traits.DeviceTypes.Virtual)]
        public async Task Stop_Monitor(SensorSpeed sensorSpeed)
        {
            if (DeviceInfo.DeviceType == DeviceType.Virtual && DeviceInfo.Platform == DeviceInfo.Platforms.iOS)
            {
                return;
            }

            var tcs = new TaskCompletionSource<CompassData>();
            Compass.Monitor(sensorSpeed, (data) =>
            {
                tcs.TrySetResult(data);
            });

            var d = await tcs.Task;

            Compass.StopMonitor();

            Assert.False(Compass.IsMonitoring);
        }
    }
}
