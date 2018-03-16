using System;
using System.Threading.Tasks;
using Microsoft.Caboodle;
using Xunit;

namespace Caboodle.DeviceTests
{
    public class Sms_Tests
    {
        [Fact]
        public Task Sms_Is_Supported()
        {
            return Utils.OnMainThread(() =>
            {
                if (Utils.IsiOSSimulator)
                    Assert.False(Sms.IsComposeSupported);
                else
                    Assert.True(Sms.IsComposeSupported);
            });
        }

        [Fact]
        public Task Sms_ComposeAsync_Throws_When_Empty()
        {
            var message = new SmsMessage();

            return Assert.ThrowsAsync<ArgumentException>(() => Sms.ComposeAsync(message));
        }

        [Fact]
        public Task Sms_ComposeAsync_Throws_With_Null_Body()
        {
            var message = new SmsMessage
            {
                Recipient = "something"
            };

            return Assert.ThrowsAsync<ArgumentException>("Body", () => Sms.ComposeAsync(message));
        }

        [Fact]
        public Task Sms_ComposeAsync_Throws_With_Null_Recipient()
        {
            var message = new SmsMessage
            {
                Body = "something"
            };

            return Assert.ThrowsAsync<ArgumentException>("Recipient", () => Sms.ComposeAsync(message));
        }
    }
}
