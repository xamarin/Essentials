using System;
using Microsoft.Caboodle;
using Xunit;

namespace Caboodle.Tests
{
    public class Compass_Tests
    {
        public Compass_Tests()
        {
            Compass.StopMonitor();
            Compass.DisposeToken();
        }

        [Fact]
        public void IsSupported_On_NetStandard() =>
            Assert.Throws<NotImplementedInReferenceAssemblyException>(() => Compass.IsSupported);

        [Fact]
        public void Monitor_Null_Handler_On_NetStandard() =>
            Assert.Throws<ArgumentNullException>(() => Compass.Monitor(SensorSpeed.Normal, null));

        [Fact]
        public void Monitor_On_NetStandard() =>
            Assert.Throws<NotImplementedInReferenceAssemblyException>(() => Compass.Monitor(SensorSpeed.Normal, (data) => { }));

        [Fact]
        public void IsMonitoring_Default_On_NetStandard() =>
            Assert.False(Compass.IsMonitoring);

        [Fact]
        public void IsMonitoring_NetStandard()
        {
            Compass.CreateToken();
            Assert.True(Compass.IsMonitoring);
        }

        [Fact]
        public void Dispose_Token_NetStandard()
        {
            Compass.CreateToken();
            Compass.DisposeToken();
            Assert.Null(Compass.MonitorCTS);
        }

        [Fact]
        public void Stop_Monitor_NetStandard()
        {
            Compass.CreateToken();
            Compass.StopMonitor();
            Assert.False(Compass.IsMonitoring);
        }
    }
}
