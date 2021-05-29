using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Shouldly;
using Xunit;

namespace TorrentFileCreator.Tests
{
    public class TorrentTests
    {
        private DirectoryInfo GetTestContentFolder([CallerFilePath] string callerFilePath = "")
        {
            return new(Path.Combine(callerFilePath, "../../../test-content"));
        }
        
        [Fact]
        public void Create_TestContentFolder_CorrectTorrent()
        {
            var md5 = MD5.Create();
            var fileStream = new MemoryStream();
            var torrent = new Torrent(new StreamWriter(fileStream), null);
            torrent.Create(GetTestContentFolder().FullName);

            var torrentFileHashValue = Convert.ToBase64String(md5.ComputeHash(fileStream.ToArray()));
            
            torrentFileHashValue.ShouldBe("lKT0ZBMYgv3OzArI6Utytw==");
        }
    }
}