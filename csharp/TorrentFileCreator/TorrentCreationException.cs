using System;

namespace TorrentFileCreator
{
    public class TorrentCreationException : Exception
    {
        public TorrentCreationException(string msg) : base(msg)
        {
        }
    }
}