using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace TorrentFileCreator
{
    public class Torrent
    {
        private FileInfo[] _fileList;

        private readonly StreamWriter _torrentFile;
        public int PieceCount { get; private set; }

        public Torrent(StreamWriter streamWriter)
        {
            _torrentFile = streamWriter;
        }

        private void BuildFileList(DirectoryInfo sourceDir)
        {
            int length = sourceDir.FullName.Split(Path.DirectorySeparatorChar).Length;
            _torrentFile.Write("5:files");
            _torrentFile.Write("l");

            _fileList = sourceDir.GetFiles("*.*", SearchOption.AllDirectories);
            
            foreach (FileInfo file in _fileList)
            {
                _torrentFile.Write("d");
                _torrentFile.Write("6:length");
                _torrentFile.Write("i");
                _torrentFile.Write(file.Length);
                _torrentFile.Write("e");
                _torrentFile.Write("4:path");
                _torrentFile.Write("l");
                string[] strArray = file.FullName.Split(Path.DirectorySeparatorChar);
                for (int index = length; index < strArray.Length; ++index)
                    _torrentFile.Write(Convert.ToString(strArray[index].Length) + ":" + strArray[index]);
                _torrentFile.Write("e");
                _torrentFile.Write("e");
            }

            _torrentFile.Write("e");
        }

        public async Task CreateAsync(string sourceDirectory)
        {
            _torrentFile.Write("d");
            _torrentFile.Write("8:announce");
            string url = "http://tracker/announce";
            _torrentFile.Write(Convert.ToString(url.Length) + ":" + url);
            _torrentFile.Write("4:info");
            _torrentFile.Write("d");
            var directoryInfo = new DirectoryInfo(sourceDirectory);
            BuildFileList(directoryInfo);
            _torrentFile.Write("4:name");
            _torrentFile.Write(directoryInfo.Name.Length + ":" + directoryInfo.Name);
            _torrentFile.Write("12:piece length");
            var pieceSize = CalculatePieceSize();
            _torrentFile.Write($"i{pieceSize}e");
            _torrentFile.Write("6:pieces");
            await WriteHashPieces(pieceSize);
            _torrentFile.Write("7:privatei1e");
            _torrentFile.Write("ee");
            _torrentFile.Close();
        }

        private int CalculatePieceSize()
        {
            double pieceSize = 32768.0;
            const double idealAmountOfPieces = 1500.0;

            long num = _fileList.Sum(file => file.Length);
            do
            {
                pieceSize *= 2;
            } while (num / pieceSize >= idealAmountOfPieces);

            return Convert.ToInt32(pieceSize);
        }

        private async Task WriteHashPieces(int pieceSize)
        {
            long totalSize = _fileList.Sum(file => file.Length);

            int pieceCount = (int) Math.Ceiling((double) totalSize / pieceSize);
            PieceCount = pieceCount;
            _torrentFile.Write(Convert.ToString(pieceCount * 20) + ":");
            _torrentFile.Flush();

            BinaryWriter binaryWriter = new BinaryWriter(_torrentFile.BaseStream);
            SHA1 shA1 = new SHA1Managed();
            int count = 0;
            int offset = 0;

            using BlockingCollection<byte[]> blockingCollection = new()
            {
                new byte[pieceSize], 
                new byte[pieceSize]
            };

            int i = 0;
            
            Task writeHashTask = Task.CompletedTask;
            var buffer1 = blockingCollection.Take();
            foreach (var file in _fileList)
            {
                var openRead = File.OpenRead(file.FullName);
                bool flag;
                do
                {
                    count = openRead.Read(buffer1, offset, pieceSize - offset);
                    if (count == pieceSize || offset + count == pieceSize)
                    {
                        await writeHashTask;
                        writeHashTask = WriteHash(shA1, buffer1, binaryWriter, blockingCollection);
                        buffer1 = blockingCollection.Take();
                        offset = 0;
                        flag = false;
                    }
                    else
                    {
                        offset += count;
                        count = offset;
                        flag = true;
                    }
                } while (!flag);
            }
            
            if (count != 0)
            {
                await writeHashTask;
                byte[] hash = shA1.ComputeHash(buffer1, 0, count);
                binaryWriter.Write(hash);
            }
        }

        private static async Task WriteHash(SHA1 shA1, byte[] array, BinaryWriter binaryWriter,
            BlockingCollection<byte[]> blockingCollection)
        {
            await Task.Run(() =>
            {
                Span<byte> hash = stackalloc byte[20];
                shA1.TryComputeHash(array, hash, out _);
              
                binaryWriter.Write(hash);
                blockingCollection.Add(array);
            });
        }
    }
}
