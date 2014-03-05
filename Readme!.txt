.NET 4.x required!
Source: git@github.com:tualatin/winmd5checksum.git
Known issues: https://github.com/tualatin/winmd5checksum/issues
If there are problems, winmd5checksum creates a error logfile in the directory. Please send me this error file bevore restart winmd5checksum, because the program deletes the error log file when it is started

Changelog:

1.0.5172.x
* improve MainWindow source
* add error logger
* delete the 256x256 image from icon, because this can cause problem in Windows XP
* SHA256 algorithm was wrong, bug fix
* add SHA512 hash