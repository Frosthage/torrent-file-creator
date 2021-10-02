package torrent

import (
	"fmt"
	"io"
	"math"
	"os"
	"strconv"
)

type pieces struct {
	Files      []os.FileInfo
	PieceSize  uint64
	SrcDirSize int64
}

func (p pieces) Write(w io.Writer) error {
	pieceCount := math.Ceil(float64(p.SrcDirSize) / float64(p.PieceSize))
	_, err := fmt.Fprintf(w, "%s:", strconv.Itoa(int(pieceCount)))
	if err != nil {
		return nil
	}
	buffer := make([]byte, p.PieceSize)
	for _, f := range p.Files {
		writeFile(f, &buffer)
	}
	return nil
}

func writeFile(w io.Writer, fileInfo os.FileInfo, buffer *[]byte, offset int64) ( int, error ){
	file, err := os.Open(fileInfo.Name())
	defer file.Close()
	if err != nil {
		return 0, err
	}
	at, err := file.ReadAt(*buffer, offset)


}


