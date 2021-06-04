using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
            //return new(@"F:\test-content");
        }

        private readonly DirectoryInfo _testContentFolder;
        private MemoryStream _memoryStream;
        private StreamWriter _streamWriter;

        public TorrentBenchmarks()
        {
            _testContentFolder = GetTestContentFolder();
            //new MemoryStream()
            //new StreamWriter()
            //var torrent = new Torrent()
        }
        
        [IterationSetup]
        public void Setup()
        {
            _memoryStream = new MemoryStream();
            _streamWriter = new StreamWriter(_memoryStream);
        }
        
        [Benchmark]
        public async Task TestContentFolder()
        {
            var torrent = new Torrent(_streamWriter);
            await torrent.CreateAsync(_testContentFolder.FullName);
        }
        
        [IterationCleanup]
        public void Cleanup()
        {
            _streamWriter.Dispose();
            _memoryStream.Dispose();
        }
    }
}