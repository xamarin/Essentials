using System;
using System.Threading.Tasks;
using Microsoft.Caboodle;
using Xunit;

namespace Caboodle.DeviceTests
{
    public class Magnetometer_Tests
    {
        bool TestSupported =>
            DeviceInfo.Platform == DeviceInfo.Platforms.Android ||
            (DeviceInfo.DeviceType == DeviceType.Physical && DeviceInfo.Platform == DeviceInfo.Platforms.iOS);

        public Magnetometer_Tests()
        {
            Magnetometer.Stop();
        }

        [Fact]
        public void IsSupported()
        {
            if (!TestSupported)
            {
                Assert.False(Magnetometer.IsSupported);
                return;
            }

            Assert.True(Magnetometer.IsSupported);
        }

        [Fact]
        public void Monitor_Null_Handler()
        {
            if (!TestSupported)
            {
                return;
            }

            Assert.Throws<ArgumentNullException>(() => Magnetometer.Start(SensorSpeed.Normal, null));
        }

        [Theory]
        [InlineData(SensorSpeed.Fastest)]
        public async Task Monitor(SensorSpeed sensorSpeed)
        {
            if (!TestSupported)
            {
                return;
            }

            var tcs = new TaskCompletionSource<MagnetometerData>();
            Magnetometer.Start(sensorSpeed, (data) =>
            {
                tcs.TrySetResult(data);
            });

            var d = await tcs.Task;

            Assert.True(d.MagneticFieldX != 0);
            Assert.True(d.MagneticFieldY != 0);
            Assert.True(d.MagneticFieldZ != 0);
        }

        [Theory]
        [InlineData(SensorSpeed.Fastest)]
        public async Task IsMonitoring(SensorSpeed sensorSpeed)
        {
            if (!TestSupported)
            {
                return;
            }

            var tcs = new TaskCompletionSource<MagnetometerData>();
            Magnetometer.Start(sensorSpeed, (data) =>
            {
                tcs.TrySetResult(data);
            });

            var d = await tcs.Task;
            Assert.True(Magnetometer.IsMonitoring);
            Magnetometer.Stop();
        }

        [Theory]
        [InlineData(SensorSpeed.Fastest)]
        public async Task Stop_Monitor(SensorSpeed sensorSpeed)
        {
            if (!TestSupported)
            {
                return;
            }

            var tcs = new TaskCompletionSource<MagnetometerData>();
            Magnetometer.Start(sensorSpeed, (data) =>
            {
                tcs.TrySetResult(data);
            });

            var d = await tcs.Task;

            Magnetometer.Stop();

            Assert.False(Magnetometer.IsMonitoring);
        }
    }
}
