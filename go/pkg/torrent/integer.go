package torrent

import (
	"fmt"
	"io"
)

type Integer uint64

func (i Integer) Write(w io.Writer) error {
	// Example: i3e represents the Integer "3"
	_, err := fmt.Fprintf(w, "i%de", i)
	return err
}

func writeInteger(w io.Writer, i Integer) error {
	return nil
}







