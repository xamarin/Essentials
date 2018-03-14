﻿using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Caboodle.Tests
{
    public class FileSystem_Tests
    {
        [Fact]
        public void FileSystem_Fail_On_NetStandard()
        {
            Assert.Throws<NotImplentedInReferenceAssembly>(() => FileSystem.AppDataDirectory);
        }

        [Fact]
        public async Task OpenAppPackageFileAsync_Fail_On_NetStandard()
        {
            await Assert.ThrowsAsync<NotImplentedInReferenceAssembly>(() => FileSystem.OpenAppPackageFileAsync("filename.txt"));
        }
    }
}
