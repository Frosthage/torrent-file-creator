package torrent

import (
	"bytes"
	"github.com/matryer/is"
	"testing"
)

func TestList_Write(t *testing.T) {
	t.Run("One string element", func(t *testing.T) {
		sut := List{}
		sut = append(sut, ByteString("str_element"))
		buffer := bytes.NewBufferString("")
		err := sut.Write(buffer)
		is := is.New(t)
		is.NoErr(err)
		is.Equal(buffer.String(), "l11:str_elemente")
	})
	t.Run("One string and one Integer", func(t *testing.T) {
		sut := List{}
		sut = append(sut, ByteString("str_element"))
		sut = append(sut, Integer(123))
		buffer := bytes.NewBufferString("")
		err := sut.Write(buffer)
		is := is.New(t)
		is.NoErr(err)
		is.Equal(buffer.String(), "l11:str_elementi123ee")
	})
	t.Run("One string, one Integer and one nested list", func(t *testing.T) {
		sut := List{}
		sut = append(sut, ByteString("str_element"))
		sut = append(sut, Integer(123))
		sut = append(sut, sut)
		buffer := bytes.NewBufferString("")
		err := sut.Write(buffer)
		is := is.New(t)
		is.NoErr(err)
		is.Equal(buffer.String(), "l11:str_elementi123el11:str_elementi123eee")
	})
}

