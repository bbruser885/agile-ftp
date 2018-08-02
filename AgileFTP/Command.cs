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
        public static CmdHelp help = new CmdHelp();
        public static CmdLocalList lls = new CmdLocalList();
        public static CmdMoveLocalFile lmv = new CmdMoveLocalFile();
	public static CmdDelete rm = new CmdDelete();

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
                case "help":
                    return help;
                case "lls":
                    return lls;
                case "lmv":
                    return lmv;
		case "rm":
		    return rm;
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

    public class CmdLocalList : Command{
        public override void Execute(string[] args)
        {
            string path = args.Length >= 2 ? args[1] : Directory.GetCurrentDirectory();
            //get files and sub dirs
            string [] files = Directory.GetFiles(path);
            string[] subdirs = Directory.GetDirectories(path);
            foreach (string dir in subdirs){
                //remove path and write
                Console.WriteLine("{0}", dir.Replace(path,""));
            }
            foreach (string file in files){
                //remove path and write
                Console.WriteLine("{0}", file.Replace(path,""));
            }
            //write them out
        }

        public override bool Validate(string[] args)
        {
            if (args.Length >= 1){
                try{
                    string path = args.Length >= 2 ? args[1] : Directory.GetCurrentDirectory();
                    //get files and sub dirs
                    string [] files = Directory.GetFiles(path);
                    string[] subdirs = Directory.GetDirectories(path);
                    return true;
                }catch (Exception ex){
                    string path = args.Length >= 2 ? args[1] : "";
                    Console.WriteLine("{0} not found, make sure this is a valid path", path);
                    return false;
                }
            }
            return false;
        }
    }

    public class CmdMoveLocalFile : Command{
        public override void Execute(string[] args)
        {
            File.Move(args[1], args[2]);
            Console.WriteLine("{0} has been moved to {1}", args[1], args[2]);
        }

        public override bool Validate(string[] args)
        {
            //if there is a cmd source and destination
            if (args.Length >= 3){
                //check if the file exists
                bool exists = File.Exists(args[1]);
                if (exists){
                    return true;
                }
                Console.WriteLine("{0} does not exist, make sure you have the right path and name");
                return false;
            }
            Console.WriteLine("please make sure you specify the file you want to move/rename followed by the new location/name");
            return false;
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

    public class CmdDelete : Command {
	public override void Execute(string[] args) {
	    CommandLineInterface.connection.Delete(args.Length >= 3 ? args[2] : args[1], args.Length >= 3 ? args[1] : "");
	}

	public override bool Validate(string[] args) {
	    if (args.Length < 2) {
		Console.WriteLine("Invalid use: rm [remotePath (default cwd)] [filename]");
		return false;
	    }
	    return true;
	}
    }

    public class CmdHelp : Command
    {
        public override void Execute(string[] args)
        {
			Console.WriteLine("help            command help");
			Console.WriteLine("exit            exit app");
			Console.WriteLine("ls              list directory");
			Console.WriteLine("cd              cd [path]");
			Console.WriteLine("upload          upload [localPath] [remotePath (default cwd)]");
			Console.WriteLine("download        download [localPath] [remotePath (default cwd)]");
			Console.WriteLine("rename          rename [remotePath (default cwd)] [New Filename]");
            Console.WriteLine("lls             list local directory");
            Console.WriteLine("lmv             move/rename local file [source] [destination]");
	    Console.WriteLine("rm              delete a remote path [remotePath (default cwd)] [filename]");
        }

        public override bool Validate(string[] args)
        {
            if (args.Length > 1) {
                return false;
            }
            return true;
        }
    }
}
