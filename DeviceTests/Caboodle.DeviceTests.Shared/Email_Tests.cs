using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Caboodle;
using Xunit;

namespace Caboodle.DeviceTests
{
    public class Email_Tests
    {
        [Fact]
        public void IsComposeSupported() =>
            Assert.True(Email.IsComposeSupported == true);


        [Fact]
        public void Email_SendAsync()
        {
            Email.ComposeAsync(
                new string[] { "moljac@mailinator.com", "moljac01@mailinator.com" },
                new string[] { "moljac02@mailinator.com" },
                null,
                "Caboodle Email",
                "Caboodle Email message in plain text",
                "text/plain",
                null);

            return;
        }
    }
}
