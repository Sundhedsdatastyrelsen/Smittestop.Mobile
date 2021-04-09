using System;
using System.IO;
using System.Threading.Tasks;
using NDB.Covid19.Interfaces;

namespace NDB.Covid19.Test.Mocks
{
    public class TestsFileSystemMocks : IFileSystem
    {
        public Task<Stream> OpenAppPackageFileAsync(string filename)
        {
            throw new NotImplementedException();
        }

        public string CacheDirectory => "/tmp/";
        public string AppDataDirectory => "/tmp/";
    }
}