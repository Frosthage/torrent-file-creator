package torrent

import (
	"io"
)

type Dictionary map[ByteString]Bencoder

func (d Dictionary) Write(w io.Writer) error {
	if _, err := w.Write([]byte("d")); err != nil {
		return err
	}
	for k, v := range d {
		k.Write(w)
		v.Write(w)
	}
	_, err := w.Write([]byte("e"))
	return err
}











