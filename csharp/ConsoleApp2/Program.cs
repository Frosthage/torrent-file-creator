using System;
using System.IO;
using TorrentFileCreator;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("sdfsdf");
            
            for (int i = 0; i < 1000000; i++)
            {
                using var memoryStream = new MemoryStream();
                using var streamWriter = new StreamWriter(memoryStream);
                var torrent = new Torrent(streamWriter);
                torrent.CreateAsync(@"C:\Program Files (x86)\Steam");
                Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b");
                Console.Write(i);
            }
        }
    }
}