package torrent

import (
	"io"
)

type List []Bencoder

func (l List) Write(w io.Writer) error {
	if _, err := w.Write([]byte("l")); err != nil {
		return err
	}
	for _, e := range l {
		if err := e.Write(w); err != nil {
			return err
		}
	}
	_, err := w.Write([]byte("e"))
	return err
}




