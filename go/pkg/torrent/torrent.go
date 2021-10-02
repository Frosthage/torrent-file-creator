package torrent

import (
	"io"
	"os"
	"path/filepath"
)

func Create(srcDir string, w io.Writer) error {
	header, err := getHeader(srcDir)
	if err != nil {
		return err
	}
	return header.Write(w)
}

func getHeader(srcDir string) (Dictionary, error) {
	files, err := getFiles(srcDir)
	if err != nil {
		return nil, err
	}
	srcDirSize := sumFileSize(files)
	pieceLength := calculatePieceLength(srcDirSize)
	torrentFile := Dictionary{
		"announce": ByteString("http://tracker/announce"),
		"info": Dictionary {
			"files":        fileList{
				files:  files,
				srcDir: srcDir,
			},
			"name":         ByteString(filepath.Base(srcDir)),
			"piece length": pieceLength,
			"pieces": pieces{
				Files:     files,
				PieceSize: uint64(pieceLength),
				SrcDirSize: srcDirSize,
			},
			"private": Integer(1),
		},
	}
	return torrentFile, nil
}

func calculatePieceLength(size int64) Integer {
	pieceSize := 32768.0
	const idealAmountOfPieces = 1500.0
	sizef := float64(size)
	for sizef/pieceSize > idealAmountOfPieces {
		pieceSize *= 2
	}
	return Integer(pieceSize)
}

func getFiles1(srcDir string) (int64, List, error) {
	srcDir, err := filepath.Abs(srcDir)
	if err != nil {
		return 0, nil, err
	}
	var totalSize int64 = 0
	result := List{}
	err = filepath.Walk(srcDir,
		func(path string, info os.FileInfo, err error) error {
			if err != nil {
				return err
			}
			if info.IsDir() {
				return nil
			}
			totalSize += info.Size()
			result = append(result, Dictionary{
				"length":Integer(info.Size()),
				"path": getFilePath(srcDir, path),
			})
			return nil
		})
	return totalSize, result, err
}












