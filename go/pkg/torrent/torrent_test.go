package torrent_test

import (
	"bufio"
	"bytes"
	"fmt"
	"testing"

	"github.com/Frosthage/torrent-file-creator/pkg/torrent"
)

func TestCreate(t *testing.T) {
	var buf bytes.Buffer
	writer := bufio.NewWriter(&buf)
	err := torrent.Create("../../../test-content", writer)
	if err != nil {
		fmt.Println(":;olkdfgdkfg")
	} else {
		fmt.Println(buf.String())
	}
}
