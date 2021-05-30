using System.IO;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using TorrentFileCreator;

namespace TorrentFileCreate.Benchmarks
{
    [MemoryDiagnoser]
    public class TorrentBenchmarks
    {
        private DirectoryInfo GetTestContentFolder([CallerFilePath] string callerFilePath = "")
        {
            return new(Path.Combine(callerFilePath, "../../../test-content"));
        }

        private readonly DirectoryInfo _testContentFolder;
        
        public TorrentBenchmarks()
        {
            _testContentFolder = GetTestContentFolder();
            //new MemoryStream()
            //new StreamWriter()
            //var torrent = new Torrent()
        }
        
        
        [Benchmark]
        public void Create()
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);
            var torrent = new Torrent(streamWriter);
            torrent.Create(_testContentFolder.FullName);
        }
    }
}