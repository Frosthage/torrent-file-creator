package torrent

import (
	"bytes"
	"errors"
	"github.com/matryer/is"
	"testing"
)

func TestByteString_Write(t *testing.T) {
	t.Run("Happy path", func(t *testing.T) {
		buffer := bytes.NewBufferString("")
		sut := ByteString("string")
		err := sut.Write(buffer)
		is := is.New(t)
		is.Equal(buffer.String(), "6:string")
		is.NoErr(err)
	})
	t.Run("Non ascii characters", func(t *testing.T) {
		buffer := bytes.NewBufferString("")
		sut := ByteString("洪流文件创建")
		err := sut.Write(buffer)
		is := is.New(t)
		is.Equal(err, errors.New("non ascii characters are not supported"))
	})
}

