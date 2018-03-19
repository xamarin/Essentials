using Microsoft.Caboodle;
using Xunit;

namespace Caboodle.Tests
{
    public class DataTransfer_Tests
    {
        [Fact]
        public async System.Threading.Tasks.Task Show_Share_UI_Text_NetStandard() =>
            await Assert.ThrowsAsync<NotImplementedInReferenceAssemblyException>(() => DataTransfer.ShowShareUI("Text"));

        [Fact]
        public async System.Threading.Tasks.Task Show_Share_UI_Text_Title_NetStandard() =>
            await Assert.ThrowsAsync<NotImplementedInReferenceAssemblyException>(() => DataTransfer.ShowShareUI("Text", "Title"));

        [Fact]
        public async System.Threading.Tasks.Task Show_Share_UI_Request_NetStandard() =>
            await Assert.ThrowsAsync<NotImplementedInReferenceAssemblyException>(() => DataTransfer.ShowShareUI(new ShareTextRequest()));
    }
}
