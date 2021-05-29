using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;

namespace TorrentUploader
{
  internal class Torrent
  {
    private string[] fileList;
    private int pieceCount;
    private int pieceSize;
    private StreamWriter torrentFile;
    private string m_PassKey;

    public int PieceCount
    {
      get => this.pieceCount;
      set => this.pieceCount = value;
    }

    public Torrent(string dest, string passkey)
    {
      try
      {
        this.torrentFile = new StreamWriter(dest);
        this.m_PassKey = passkey;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
      this.pieceSize = 32768;
    }

    private void BuildFileList(string dirDest)
    {
      int length = dirDest.Split('\\').Length;
      this.torrentFile.Write("5:files");
      this.torrentFile.Write("l");
      this.fileList = Directory.GetFiles(dirDest, "*.*", SearchOption.AllDirectories);
      Array.Sort((Array) this.fileList, (IComparer) new Torrent.FileDestComparer());
      foreach (string file in this.fileList)
      {
        this.torrentFile.Write("d");
        this.torrentFile.Write("6:length");
        this.torrentFile.Write("i");
        this.torrentFile.Write(new FileInfo(file).Length);
        this.torrentFile.Write("e");
        this.torrentFile.Write("4:path");
        this.torrentFile.Write("l");
        string[] strArray = file.Split('\\');
        for (int index = length; index < strArray.Length; ++index)
          this.torrentFile.Write(Convert.ToString(strArray[index].Length) + ":" + strArray[index]);
        this.torrentFile.Write("e");
        this.torrentFile.Write("e");
      }
      this.torrentFile.Write("e");
    }

    public void BuilTorrent(BackgroundWorker bw, string dirDest)
    {
      string[] strArray = dirDest.Split('\\');
      try
      {
        this.torrentFile.Write("d");
        this.torrentFile.Write("8:announce");
        string str = "http://tracker/announce";
        this.torrentFile.Write(Convert.ToString(str.Length) + ":" + str);
        this.torrentFile.Write("4:info");
        this.torrentFile.Write("d");
        this.BuildFileList(dirDest);
        this.torrentFile.Write("4:name");
        this.torrentFile.Write(strArray[strArray.Length - 1].Length.ToString() + ":" + strArray[strArray.Length - 1]);
        this.torrentFile.Write("12:piece length");
        this.CalculatePieceSize();
        this.torrentFile.Write("i" + this.pieceSize.ToString() + "e");
        this.torrentFile.Write("6:pieces");
        this.WriteHashPieces(bw, dirDest);
        this.torrentFile.Write("7:privatei1e");
        this.torrentFile.Write("ee");
        this.torrentFile.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private string CalculatePieceSize()
    {
      long num = 0;
      foreach (string file in this.fileList)
        num += new FileInfo(file).Length;
      do
      {
        this.pieceSize *= 2;
      }
      while ((double) (num / (long) this.pieceSize) >= 1500.0);
      return this.pieceSize.ToString();
    }

    private void WriteHashPieces(BackgroundWorker bw, string dirDest)
    {
      long num1 = 0;
      int num2 = 0;
      foreach (string file in this.fileList)
        num1 += new FileInfo(file).Length;
      int num3 = (int) Math.Ceiling((double) num1 / (double) this.pieceSize);
      this.PieceCount = num3;
      this.torrentFile.Write(Convert.ToString(num3 * 20) + ":");
      this.torrentFile.Flush();
      BinaryWriter binaryWriter = new BinaryWriter(this.torrentFile.BaseStream);
      byte[] buffer1 = new byte[this.pieceSize];
      SHA1 shA1 = (SHA1) new SHA1Managed();
      int count = 0;
      int offset = 0;
      DateTime now = DateTime.Now;
      FileInfo fileInfo1 = new FileInfo(this.fileList[0]);
      FileStream fileStream = fileInfo1.OpenRead();
      byte[] buffer2 = new byte[fileInfo1.Length];
      IAsyncResult asyncResult = fileStream.BeginRead(buffer2, 0, (int) fileInfo1.Length, (AsyncCallback) null, (object) null);
      for (int index = 1; index <= this.fileList.Length; ++index)
      {
        fileStream.EndRead(asyncResult);
        fileStream.Close();
        MemoryStream memoryStream = new MemoryStream(buffer2);
        if (this.fileList.Length > index)
        {
          FileInfo fileInfo2 = new FileInfo(this.fileList[index]);
          buffer2 = new byte[fileInfo2.Length];
          fileStream = fileInfo2.OpenRead();
          asyncResult = fileStream.BeginRead(buffer2, 0, (int) fileInfo2.Length, (AsyncCallback) null, (object) null);
        }
        bool flag;
        do
        {
          count = memoryStream.Read(buffer1, offset, this.pieceSize - offset);
          if (count == this.pieceSize || offset + count == this.pieceSize)
          {
            byte[] hash = shA1.ComputeHash(buffer1);
            binaryWriter.Write(hash);
            ++num2;
            double num4 = Math.Round((double) num2 / (double) this.PieceCount * 100.0);
            bw.ReportProgress(Convert.ToInt32(num4));
            offset = 0;
            flag = false;
          }
          else
          {
            offset += count;
            count = offset;
            flag = true;
          }
        }
        while (!flag);
      }
      if (count != 0)
      {
        byte[] hash = shA1.ComputeHash(buffer1, 0, count);
        binaryWriter.Write(hash);
      }
      bw.ReportProgress(100);
    }

    private class FileDestComparer : IComparer
    {
      public int Compare(object x, object y)
      {
        string[] strArray1 = (x as string).Split('\\');
        string[] strArray2 = (y as string).Split('\\');
        int num = Math.Min(strArray1.Length, strArray2.Length);
        for (int index = 0; index < num - 1; ++index)
        {
          if (strArray1[index] != strArray2[index])
            return string.Compare(strArray1[index], strArray2[index]);
        }
        if (strArray1.Length == strArray2.Length)
          return string.Compare(strArray1[strArray1.Length - 1], strArray2[strArray2.Length - 1]);
        return strArray1.Length < strArray2.Length ? 1 : -1;
      }
    }
  }
}
