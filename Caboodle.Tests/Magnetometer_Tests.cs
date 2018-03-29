using System;
using Microsoft.Caboodle;
using Xunit;

namespace Caboodle.Tests
{
    public class Magnetometer_Tests
    {
        public Magnetometer_Tests()
        {
            Magnetometer.Stop();
            Magnetometer.DisposeToken();
        }

        [Fact]
        public void IsSupported_On_NetStandard() =>
            Assert.Throws<NotImplementedInReferenceAssemblyException>(() => Magnetometer.IsSupported);

        [Fact]
        public void Monitor_Null_Handler_On_NetStandard() =>
            Assert.Throws<ArgumentNullException>(() => Magnetometer.Start(SensorSpeed.Normal, null));

        [Fact]
        public void Monitor_On_NetStandard() =>
            Assert.Throws<NotImplementedInReferenceAssemblyException>(() => Magnetometer.Start(SensorSpeed.Normal, (data) => { }));

        [Fact]
        public void IsMonitoring_Default_On_NetStandard() =>
            Assert.False(Magnetometer.IsMonitoring);

        [Fact]
        public void IsMonitoring_NetStandard()
        {
            Magnetometer.CreateToken();
            Assert.True(Magnetometer.IsMonitoring);
        }

        [Fact]
        public void Dispose_Token_NetStandard()
        {
            Magnetometer.CreateToken();
            Magnetometer.DisposeToken();
            Assert.Null(Magnetometer.MonitorCTS);
        }

        [Fact]
        public void Stop_Monitor_NetStandard()
        {
            Magnetometer.CreateToken();
            Magnetometer.Stop();
            Assert.False(Magnetometer.IsMonitoring);
        }
    }
}
