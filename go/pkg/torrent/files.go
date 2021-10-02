package torrent

import (
	"io"
	"os"
	"path/filepath"
	"strings"
)

type fileList struct {
	files  []os.FileInfo
	srcDir string
}

func (fl fileList) Write(w io.Writer) error {
	_, err := w.Write([]byte("l"))
	if err != nil {
		return err
	}
	for _, f := range fl.files {
		err := Dictionary{
			"length":Integer(f.Size()),
			"path": getFilePath(fl.srcDir, f.Name()),
		}.Write(w)
		if err != nil {
			return err
		}
	}
	_, err = w.Write([]byte("e"))
	if err != nil {
		return err
	}
	return nil
}


/*func getFiles1(srcDir string) (int64, List, error) {
	srcDir, err := filepath.Abs(srcDir)
	if err != nil {
		return 0, nil, err
	}
	var totalSize int64 = 0
	result := List{}
	err = filepath.Walk(srcDir,
		func(path string, info os.FileInfo, err error) error {
			if err != nil {
				return err
			}
			if info.IsDir() {
				return nil
			}
			totalSize += info.Size()
			result = append(result, Dictionary{
				"length":Integer(info.Size()),
				"path": getFilePath(srcDir, path),
			})
			return nil
		})
	return totalSize, result, err
}
*/


func getFilePath(srcDir string, path string) List {
	split := strings.Split(srcDir, string([]rune{filepath.Separator}))
	split2 := strings.Split(path, string([]rune{filepath.Separator}))
	result := List{}
	for i := len(split); i < len(split2); i++ {
		result = append(result, ByteString(split2[i]))
	}
	return result
}



func getFiles(srcDir string) ([]os.FileInfo, error) {
	var result []os.FileInfo
	err := filepath.Walk(srcDir,
		func(path string, info os.FileInfo, err error) error {
			if err != nil {
				return err
			}
			if !info.IsDir() {
				result = append(result, info)
			}
			return nil
		})
	return result, err
}

func sumFileSize(files []os.FileInfo) int64 {
	var sum int64
	for _, f := range files {
		sum += f.Size()
	}
	return sum
}
