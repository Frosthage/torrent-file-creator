package torrent

import "io"

type Bencoder interface {
	Write(w io.Writer) error
}

