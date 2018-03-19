using Microsoft.Caboodle;
using Xunit;

namespace Caboodle.Tests
{
    public class Email_Tests
    {
        [Fact]
        public void Email_OnSendSuccess_Event_On_NetStandard() =>
            Assert.Throws<NotImplementedInReferenceAssemblyException>(() => Email.OnSendSuccess += Email_OnSendSuccess);

        [Fact]
        public void Email_OnSendError_Event_On_NetStandard() =>
            Assert.Throws<NotImplementedInReferenceAssemblyException>(() => Email.OnSendError += Email_OnSendSuccess);


        void Email_OnSendSuccess(EmailSendEventArgs e)
        {
        }

        void Email_OnSendError(EmailSendEventArgs e)
        {
        }
    }
}
