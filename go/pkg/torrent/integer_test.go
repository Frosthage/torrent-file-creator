package torrent

import (
	"bytes"
	"github.com/matryer/is"
	"testing"
)


func TestInteger_Write(t *testing.T) {
	t.Run("Happy path", func(t *testing.T) {
		buffer := bytes.NewBufferString("")
		sut := Integer(123)
		err := sut.Write(buffer)
		is := is.New(t)
		is.Equal(buffer.String(), "i123e")
		is.NoErr(err)
	})
}







