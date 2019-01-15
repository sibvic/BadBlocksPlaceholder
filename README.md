# BadBlocksPlaceholder
Places files on HDD's bad blocks. Fill all available space on the selected disk with files and then reads them. 
If the file reads without any errors then it will be deleted. 
If we will get any error it will be left forever on the hard disk in the BadBlockPlaceholders/yyyyMMdd folder.

It accepts two parameters: the disk drive and the block size in KB (file of the file to create). For example:

    BadBlocksPlaceholder e:\ 1024

This will run the test using 1MB files.
