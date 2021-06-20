package torrent

import (
	"errors"
	"fmt"
	"io"
	"unicode"
)

type ByteString string

func (bs ByteString) Write(w io.Writer) error {
	for _, c := range bs {
		if c > unicode.MaxASCII {
			return errors.New("non ascii characters are not supported")
		}
	}
	// <string length encoded in base ten ASCII>:<string data>
	_, err := fmt.Fprintf(w, "%d:%v", len(bs), bs)
	return err
}

