using System;
using System.IO;
using FtpConnection;
using System.Timers;

namespace AgileFTP {
    public static class CommandLineInterface {

        public static FtpConnectionManager connection;
        public static bool running;
        public static Timer timeoutTimer;

        public static void Start() {
            timeoutTimer = new Timer();
            timeoutTimer.Interval = 60000; //TODO decide on the timeout interval temporarily set to one minute

            timeoutTimer.Elapsed += OnTimeoutEvent;
            timeoutTimer.Enabled = true;
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
                Console.Write(connection.GetCWD() + ":");
                String cmd = Console.ReadLine();
                ParseCommand(cmd);
            }
        }

        private static void ParseCommand(string cmd) {

            string[] args = cmd.ToLower().Split(' ');

            if (args.Length < 1) {
                return;
            }

            ResetTimer();

            Command c = Command.GetCommand(args[0]);
            if (c == null) {
                Console.WriteLine("Command not found.");
            } else if (c.Validate(args)) {
                c.Execute(args);
            }
        }

        private static void OnTimeoutEvent(Object source, ElapsedEventArgs e) {
            Console.WriteLine("timeout");
            Environment.Exit(1);
        }

        private static void ResetTimer() {
            timeoutTimer.Stop();
            timeoutTimer.Start();
        }
    }
}
