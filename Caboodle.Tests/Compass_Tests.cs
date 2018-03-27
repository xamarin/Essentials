using System;
using Microsoft.Caboodle;
using Xunit;

namespace Caboodle.Tests
{
    public class Compass_Tests
    {
        [Fact]
        public void Monitor_Null_Handler_On_NetStandard() =>
            Assert.Throws<ArgumentNullException>(() => Compass.Monitor(SensorSpeed.Normal, null));

        [Fact]
        public void Monitor_On_NetStandard() =>
            Assert.Throws<NotImplementedInReferenceAssemblyException>(() => Compass.Monitor(SensorSpeed.Normal, (data) => { }));

        [Fact]
        public void IsMonitoring_Default_On_NetStandard() =>
            Assert.False(Compass.IsMonitoring);
    }
}
