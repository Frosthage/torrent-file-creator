package torrent_test

import (
	"bufio"
	"fmt"
	"os"
	"testing"

	"github.com/Frosthage/torrent-file-creator/pkg/torrent"
)

func TestCreate(t *testing.T) {
	f, _ := os.Create("apa.log")
	defer f.Close()
	writer := bufio.NewWriter(f)
	err := torrent.Create("sdfsdf", writer)
	if err != nil {
		fmt.Println(":;olkdfgdkfg")
	} else {
		fmt.Println("ffffffffffff")
	}
}
