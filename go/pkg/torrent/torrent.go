package torrent

import (
	"io"
)

type String string



func Create(sourceDir string, w io.Writer) error {
	return nil





	//string[] strArray = sourceDirectory.Split('\\')
	//_torrentFile.Write("d")
	//_torrentFile.Write("8:announce")
	//string url = "http://tracker/announce"
	//_torrentFile.Write(Convert.ToString(url.Length) + ":" + url)
	//_torrentFile.Write("4:info")
	//_torrentFile.Write("d")
	//BuildFileList(sourceDirectory)
	//_torrentFile.Write("4:name")
	//_torrentFile.Write(strArray[^1].Length + ":" + strArray[^1])
	//_torrentFile.Write("12:piece length")
	//var pieceSize = CalculatePieceSize()
	//_torrentFile.Write($"i{pieceSize}e")
	//_torrentFile.Write("6:pieces")
	//await WriteHashPieces(pieceSize)
	//_torrentFile.Write("7:privatei1e")
	//_torrentFile.Write("ee")
	//_torrentFile.Close()


}

func getHeader() map[ByteString]Bencoder{
	header := Dictionary{
		"announce": ByteString("http://tracker/announce"),
		"info": Dictionary{
			
		},

	}
	header["announce"] = ByteString("http://tracker/announce")
	return header

}

