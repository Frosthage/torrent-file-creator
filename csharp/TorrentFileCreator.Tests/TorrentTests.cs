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

            var torrentFileHashValue = Convert.ToBase64String(md5.ComputeHash(fileStream.ToArray()));
            
            torrentFileHashValue.ShouldBe("lKT0ZBMYgv3OzArI6Utytw==");
        }
        
        [Fact]
        public async Task Bepurt()
        {
            var md5 = MD5.Create();
            var fileStream = new MemoryStream();
            var torrent = new Torrent(new StreamWriter(fileStream));
            await torrent.CreateAsync(@"C:\Program Files (x86)\Steam");

            var torrentFileHashValue = Convert.ToBase64String(md5.ComputeHash(fileStream.ToArray()));
            
            torrentFileHashValue.ShouldBe("lKT0ZBMYgv3OzArI6Utytw==");
        }
        
        [Fact]
        public async Task asfasdfasdfas()
        {
            var md5 = MD5.Create();
            var fileStream = File.OpenWrite(@"c:\slask\unsorted.torrent");
            var torrent = new Torrent(new StreamWriter(fileStream));
            await torrent.CreateAsync(@"C:\Program Files (x86)\Steam");

            //var torrentFileHashValue = Convert.ToBase64String(md5.ComputeHash(fileStream.ToArray()));
            
 //1           torrentFileHashValue.ShouldBe("lKT0ZBMYgv3OzArI6Utytw==");
        }
        
        [Fact]
        public async Task TEstststy()
        {
            var md5 = MD5.Create();
            var fileStream = File.OpenWrite(@"test.torrent");
            var torrent = new Torrent(new StreamWriter(fileStream));
            await torrent.CreateAsync(GetTestContentFolder().FullName);
            
            //var torrentFileHashValue = Convert.ToBase64String(md5.ComputeHash(fileStream.ToArray()));
            
            //1           torrentFileHashValue.ShouldBe("lKT0ZBMYgv3OzArI6Utytw==");
        }
    }
}