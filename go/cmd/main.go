package main

import (
	"bufio"
	"fmt"
	"os"

	"github.com/Frosthage/torrent-file-creator/pkg/torrent"
)

func main() {
	f, _ := os.Create("apa.log")
	defer f.Close()
	writer := bufio.NewWriter(f)
	err := torrent.Create("sdfsdf", writer)
	if err != nil {
		fmt.Println(fmt.Errorf("apa %w", err))
	} else {
		fmt.Println("Okeeeij!")
	}
}
