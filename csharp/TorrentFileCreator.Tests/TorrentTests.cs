using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
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
        public async Task Create_TestContentFolder_CorrectTorrent()
        {
            var md5 = MD5.Create();
            var fileStream = new MemoryStream();
            var torrent = new Torrent(new StreamWriter(fileStream));
            await torrent.CreateAsync(GetTestContentFolder().FullName);

            await using var stream = new FileStream("apa2.torrent", FileMode.Create);
            stream.Write(fileStream.GetBuffer());


            var torrentFileHashValue = Convert.ToBase64String(md5.ComputeHash(fileStream.ToArray()));
            
            torrentFileHashValue.ShouldBe("lKT0ZBMYgv3OzArI6Utytw==");
        }
    }
}