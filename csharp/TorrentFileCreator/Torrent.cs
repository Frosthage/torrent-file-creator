using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace TorrentFileCreator
{
  public class Torrent
  {
    private string[] _fileList;

    private readonly StreamWriter _torrentFile;
    public int PieceCount { get; private set; }

    public Torrent(StreamWriter streamWriter)
    {
      _torrentFile = streamWriter;
    }

    private void BuildFileList(string dirDest)
    {
      int length = dirDest.Split('\\').Length;
      _torrentFile.Write("5:files");
      _torrentFile.Write("l");
      _fileList = Directory.GetFiles(dirDest, "*.*", SearchOption.AllDirectories);
      Array.Sort(_fileList, new FileDestComparer());
      foreach (string file in _fileList)
      {
        _torrentFile.Write("d");
        _torrentFile.Write("6:length");
        _torrentFile.Write("i");
        _torrentFile.Write(new FileInfo(file).Length);
        _torrentFile.Write("e");
        _torrentFile.Write("4:path");
        _torrentFile.Write("l");
        string[] strArray = file.Split('\\');
        for (int index = length; index < strArray.Length; ++index)
          _torrentFile.Write(Convert.ToString(strArray[index].Length) + ":" + strArray[index]);
        _torrentFile.Write("e");
        _torrentFile.Write("e");
      }

      _torrentFile.Write("e");
    }

    public void Create(string sourceDirectory)
    {
      string[] strArray = sourceDirectory.Split('\\');
      _torrentFile.Write("d");
      _torrentFile.Write("8:announce");
      string url = "http://tracker/announce";
      _torrentFile.Write(Convert.ToString(url.Length) + ":" + url);
      _torrentFile.Write("4:info");
      _torrentFile.Write("d");
      BuildFileList(sourceDirectory);
      _torrentFile.Write("4:name");
      _torrentFile.Write(strArray[^1].Length + ":" + strArray[^1]);
      _torrentFile.Write("12:piece length");
      var pieceSize = CalculatePieceSize();
      _torrentFile.Write($"i{pieceSize}e");
      _torrentFile.Write("6:pieces");
      WriteHashPieces(pieceSize);
      _torrentFile.Write("7:privatei1e");
      _torrentFile.Write("ee");
      _torrentFile.Close();
    }

    private int CalculatePieceSize()
    {
      double pieceSize = 32768.0;
      const double idealAmountOfPieces = 1500.0;

      long num = _fileList.Sum(file => new FileInfo(file).Length);
      do
      {
        pieceSize *= 2;
      } while (num / pieceSize >= idealAmountOfPieces);

      return Convert.ToInt32(pieceSize);
    }

    private void WriteHashPieces(int pieceSize)
    {
      long totalSize = _fileList.Sum(file => new FileInfo(file).Length);

      int pieceCount = (int) Math.Ceiling((double) totalSize / pieceSize);
      PieceCount = pieceCount;
      _torrentFile.Write(Convert.ToString(pieceCount * 20) + ":");
      _torrentFile.Flush();

      BinaryWriter binaryWriter = new BinaryWriter(_torrentFile.BaseStream);
      byte[] buffer1 = new byte[pieceSize];
      SHA1 shA1 = new SHA1Managed();
      int count = 0;
      int offset = 0;
      
      foreach (var file in _fileList)
      {
        var openRead = File.OpenRead(file);
        bool flag;
        do
        {
          count = openRead.Read(buffer1, offset, pieceSize - offset);
          if (count == pieceSize || offset + count == pieceSize)
          {
            WriteHash(shA1, buffer1, binaryWriter);
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
        byte[] hash = shA1.ComputeHash(buffer1, 0, count);
        binaryWriter.Write(hash);
      }
    }

    private static void WriteHash(SHA1 shA1, byte[] array, BinaryWriter binaryWriter)
    {
      Span<byte> hash = stackalloc byte[20];
      shA1.TryComputeHash(array, hash, out var written);
      if (written != 20)
      {
        throw new TorrentCreationException("Computed hash is expected to have the size of 20 bytes");
      }
      binaryWriter.Write(hash);
    }
  }
}
