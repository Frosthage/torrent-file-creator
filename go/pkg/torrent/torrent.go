package torrent

import (
	"bufio"
)

func Create(sourceDir string, writer *bufio.Writer) error {
	_, err := writer.Write([]byte("sdlfgjhsdlfgkh"))
	if err != nil {
		return err
	}
	return writer.Flush()
}

