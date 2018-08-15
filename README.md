# agile-ftp
## master: [![Build Status](https://computerthings.ddns.net/jenkins/buildStatus/icon?job=agile-ftp/master)](https://computerthings.ddns.net/jenkins/job/agile-ftp/job/master/)

### about
This is a command line ftp client.
### installation
To install this program clone the repo and build the project.
If you are running this on windows the build process should creat an agileFTPInstaller.msi file
run this to get a setup wizard that will install the application
Windows is the intended platform for this program although everything but the installer is written with .net core
This means the program should run on mac and linux as well as long as you do not use the installer.
### operation
upon starting the program you will be prompted to create a new connection.
after which you will get a command line prompt.
from that prompt you can type the following commands:
*****COMMAND******************************ARGUMENTS***************************************************DEFINITION
      help                              none                                    prints the commands you can list
      exit                              none                                    exits the application
      ls                                optional[directory path]                list the directory
      cd                                [directory path]                        change directory
      upload                            [local path]                            uploads file at local path
      download                          [local path][remote path]               downloads file from remote to local
      rename                            [remote path][new name]                 renames remote file
      lls                                optional[directory path]               list local directory
      lmv                                [file path][new name/path]             move/rename local file
      rm                                 optional[remote path][filename]        remove remote file
