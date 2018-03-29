﻿using System;
using System.Threading.Tasks;
using Microsoft.Caboodle;
using Xunit;

namespace Caboodle.DeviceTests
{
    public class Compass_Tests
    {
        bool TestSupported =>
               DeviceInfo.Platform == DeviceInfo.Platforms.Android ||
               DeviceInfo.Platform == DeviceInfo.Platforms.UWP ||
               (DeviceInfo.DeviceType == DeviceType.Physical && DeviceInfo.Platform == DeviceInfo.Platforms.iOS);

        public Compass_Tests()
        {
            Compass.Stop();
        }

        [Fact]
        public void IsSupported()
        {
            if (!TestSupported)
            {
                Assert.False(Compass.IsSupported);
                return;
            }

            Assert.True(Compass.IsSupported);
        }

        [Fact]
        public void Monitor_Null_Handler()
        {
            if (!TestSupported)
            {
                return;
            }

            Assert.Throws<ArgumentNullException>(() => Compass.Start(SensorSpeed.Normal, null));
        }

        [Theory]
        [InlineData(SensorSpeed.Fastest)]
        public async Task Monitor(SensorSpeed sensorSpeed)
        {
            if (!TestSupported)
            {
                return;
            }

            var tcs = new TaskCompletionSource<CompassData>();
            Compass.Start(sensorSpeed, (data) =>
            {
                tcs.TrySetResult(data);
            });

            var d = await tcs.Task;

            Assert.True(d.HeadingMagneticNorth >= 0);
        }

        [Theory]
        [InlineData(SensorSpeed.Fastest)]
        public async Task IsMonitoring(SensorSpeed sensorSpeed)
        {
            if (!TestSupported)
            {
                return;
            }

            var tcs = new TaskCompletionSource<CompassData>();
            Compass.Start(sensorSpeed, (data) =>
            {
                tcs.TrySetResult(data);
            });

            var d = await tcs.Task;
            Assert.True(Compass.IsMonitoring);
            Compass.Stop();
        }

        [Theory]
        [InlineData(SensorSpeed.Fastest)]
        public async Task Stop_Monitor(SensorSpeed sensorSpeed)
        {
            if (!TestSupported)
            {
                return;
            }

            var tcs = new TaskCompletionSource<CompassData>();
            Compass.Start(sensorSpeed, (data) =>
            {
                tcs.TrySetResult(data);
            });

            var d = await tcs.Task;

            Compass.Stop();

            Assert.False(Compass.IsMonitoring);
        }
    }
}
