using Microsoft.Caboodle;
using Xunit;

namespace Caboodle.Tests
{
    public class Email_Tests
    {
        [Fact]
        public void Email_OnSendSuccess_Event_On_NetStandard() =>
            Assert.Throws<NotImplentedInReferenceAssembly>(() => Email.OnSendSuccess += Email_OnSendSuccess);

        [Fact]
        public void Email_OnSendError_Event_On_NetStandard() =>
            Assert.Throws<NotImplentedInReferenceAssembly>(() => Email.OnSendError += Email_OnSendSuccess);


        void Email_OnSendSuccess(EmailSendEventArgs e)
        {
        }

        void Email_OnSendError(EmailSendEventArgs e)
        {
        }
    }
}
