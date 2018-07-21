using System;
using System.IO;

namespace AgileFTP {
    public abstract class Command {

        public abstract bool Validate(string[] args);
        public abstract void Execute(string[] args);

        public static CmdExit exit = new CmdExit();
        public static CmdList ls = new CmdList();
        public static CmdChangeDirectory cd = new CmdChangeDirectory();
        public static CmdUpload upload = new CmdUpload();
        public static CmdDownload download = new CmdDownload();
        public static CmdRename rename = new CmdRename();

        public static Command GetCommand(string s) {
            switch (s) {
                case "exit":
                    return exit;
                case "ls":
                    return ls;
                case "cd":
                    return cd;
                case "upload":
                    return upload;
                case "download":
                    return download;
                case "rename":
                    return rename;
                default: 
                    return null;
            }
        }
    }

    public class CmdExit : Command {
        public override void Execute(string[] args) {
            CommandLineInterface.running = false;
            Console.Write("Closing connection.");
        }

        public override bool Validate(string[] args) {
            return true;
        }
    }

    public class CmdList : Command {
        public override void Execute(string[] args) {
            string path = args.Length >= 2 ?  args[1] : "";
            string files = CommandLineInterface.connection.listFiles(path);
            Console.WriteLine("{0}", files);
        }

        public override bool Validate(string[] args) {
            if (args.Length == 1)
                return true;
            else {
                // validate the directory's existence?
                return true;
            }
        }
    }

    public class CmdChangeDirectory : Command {
        public override void Execute(string[] args) {
            CommandLineInterface.connection.ChangeDirectory(args[1]);
        }

        public override bool Validate(string[] args) {
            if (args.Length < 2) {
                Console.Write("Invalid use: cd [path]");
                return false;
            }
            return true;
        }
    }

    public class CmdUpload : Command {
        public override void Execute(string[] args) {
            CommandLineInterface.connection.Upload(Path.GetFileName(args[1]), args.Length >= 3 ? args[2] : "", args[1]);
        }

        public override bool Validate(string[] args) {
            if (args.Length < 2) {
                Console.Write("Invalid use: upload [localPath] [remotePath (default cwd)]");
                return false;
            }
            return true;
        }
    }

    public class CmdDownload : Command {
        public override void Execute(string[] args) {
            CommandLineInterface.connection.Download(Path.GetFileName(args[2]), args.Length >= 3 ? args[2] : "", args[1]);
        }

        public override bool Validate(string[] args) {
            if (args.Length < 2) {
                Console.WriteLine("Invalid use: download [localPath] [remotePath (default cwd)]");
                return false; 
            }
            return true;
        }
    }
    public class CmdRename : Command {
        public override void Execute(string[] args) {
            CommandLineInterface.connection.Rename(args.Length >= 3 ? args[2] : "", Path.GetFileName(args[1]), args[1]);
        }

        public override bool Validate(string[] args) {
            if (args.Length < 2) {
                Console.WriteLine("Invalid use: rename [remotePath (default cwd)] [New Filename]");
                return false;
            }
            return true;
        }
    }
}
