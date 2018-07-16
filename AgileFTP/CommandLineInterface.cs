using System;
using FtpConnection;

namespace AgileFTP {
    public static class CommandLineInterface {

        private static FtpConnectionManager connection;
        private static bool running;

        public static void Start() {
            Login();
        }

        private static void Login() {
            Console.Write("Enter hostname:");
            String h = Console.ReadLine();
            if (h == "?") {
                SkipLogin();
                return;
            }
            Console.Write("Enter username:");
            String u = Console.ReadLine();
            Console.Write("Enter password:");
            String p = Console.ReadLine();
            connection = new FtpConnectionManager(u, p, h);
            if (connection.Validate())
                ProcessInput();
            else {
                Console.WriteLine("Login Failed");
                Login();
            }
                
        }

        private static void SkipLogin() {
            connection = new FtpConnectionManager();
            if (connection.Validate())
                ProcessInput();
            else {
                Console.WriteLine("Test Login Failed");
            }
        }

        private static void ProcessInput() {
            running = true;
            while (running) {
                Console.Write(">");
                String cmd = Console.ReadLine();
                ParseCommand(cmd);
            }
        }

        private static void ParseCommand(string cmd) {
            Console.WriteLine(cmd);
        }

    }
}
