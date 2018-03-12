using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Caboodle;
using Xunit;

namespace Caboodle.DeviceTests
{
    public class Battery_Tests
    {
        [Fact]
        public void Charge_Level() =>
            Assert.InRange(Battery.ChargeLevel, 0.01, 100.0);

        [Fact]
        public void Charge_State() =>
            Assert.NotEqual(BatteryState.Unknown, Battery.State);

        [Fact]
        public void Charge_Power() =>
            Assert.NotEqual(BatteryPowerSource.Unknown, Battery.PowerSource);
    }
}
