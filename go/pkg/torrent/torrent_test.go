package torrent_test

import (
	"bufio"
	"bytes"
	"fmt"
	"os"
	"testing"

	"github.com/Frosthage/torrent-file-creator/pkg/torrent"
)

func TestCreate(t *testing.T) {
	f, _ := os.Create("apa.log")
	defer f.Close()
	var buf bytes.Buffer
	writer := bufio.NewWriter(&buf)
	err := torrent.Create("sdfsdf", writer)
	if err != nil {
		fmt.Println(":;olkdfgdkfg")
	} else {
		fmt.Println(buf.String())
	}
}
