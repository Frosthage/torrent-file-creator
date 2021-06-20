package torrent

import (
	"bytes"
	"github.com/matryer/is"
	"testing"
)

func TestDictionary_Write(t *testing.T) {
	t.Run("One ByteString value pair", func(t *testing.T) {
		sut := Dictionary{}
		sut[ByteString("Key")] = ByteString("value")
		buffer := bytes.NewBufferString("")
		err := sut.Write(buffer)
		is := is.New(t)
		is.Equal(buffer.String(), "d3:Key5:valuee")
		is.NoErr(err)
	})
	t.Run("one Integer", func(t *testing.T) {
		sut := Dictionary{}
		sut[ByteString("Key2")] = Integer(123)
		buffer := bytes.NewBufferString("")
		err := sut.Write(buffer)
		is := is.New(t)
		is.Equal(buffer.String(), "d4:Key2i123ee")
		is.NoErr(err)
	})
	t.Run("Dictionary", func(t *testing.T) {
		sut := Dictionary{}
		d := Dictionary{}
		d[ByteString("Key")] = ByteString("value")
		sut[ByteString("Key")] = d
		buffer := bytes.NewBufferString("")
		err := sut.Write(buffer)
		is := is.New(t)
		is.Equal(buffer.String(), "d3:Keyd3:Key5:valueee")
		is.NoErr(err)
	})
	t.Run("one List", func(t *testing.T) {
		sut := Dictionary{}
		l := List{}
		l = append(l, ByteString("str_element"))
		l = append(l, Integer(123))
		sut[ByteString("Key")] = l
		buffer := bytes.NewBufferString("")
		err := sut.Write(buffer)
		is := is.New(t)
		is.Equal(buffer.String(), "d3:Keyl11:str_elementi123eee")
		is.NoErr(err)
	})
	t.Run("Empty dictionary", func(t *testing.T) {
		sut := Dictionary{}
		buffer := bytes.NewBufferString("")
		err := sut.Write(buffer)
		is := is.New(t)
		is.Equal(buffer.String(), "de")
		is.NoErr(err)
	})
}






