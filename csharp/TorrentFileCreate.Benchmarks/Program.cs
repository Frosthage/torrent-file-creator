namespace TorrentFileCreate.Benchmarks
{
    using BenchmarkDotNet.Running;

    namespace MyBenchmarks
    {
        public class Program
        {
            public static void Main()
            {
                BenchmarkRunner.Run(typeof(Program).Assembly);
            }
        }
    }
}