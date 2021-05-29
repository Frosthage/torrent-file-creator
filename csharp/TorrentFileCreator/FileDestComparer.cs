using System;
using System.Collections;

namespace TorrentFileCreator
{
    internal class FileDestComparer : IComparer
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