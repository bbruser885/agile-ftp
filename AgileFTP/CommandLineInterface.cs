using System;
using System.IO;
using FtpConnection;
using System.Timers;
using System.Globalization;
using AgileFTP.Model;
using Newtonsoft.Json;

namespace AgileFTP {
    public static class CommandLineInterface {

        public static FtpConnectionManager connection;
        public static bool running;
        public static Timer timeoutTimer;
        public static string userName;
        public static string Host;
        public static string Password;
        public static string logFile;

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
            userName = u;
            logFile = userName + ".log";
            Console.Write("Enter password:");
            String p = Console.ReadLine();
            Password = p;
            connection = new FtpConnectionManager(u, p, h);
            if (connection.Validate())
            {
                Console.Write($"Save Login? y/n: ");
                String choice = Console.ReadLine();
                if (choice == "y")
                {
                    Console.Write("Save connection as: ");
                    String connectionName = Console.ReadLine();
                    connectionName.Replace(' ', '_');
                    Connection conn = new Connection();
                    conn.User = userName;
                    conn.Host = Host;
                    conn.Password = Password;

                    using (StreamWriter file = File.CreateText($"{connectionName}.txt"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, conn);
                    }
                }
                ProcessInput();
            }
            else {
                Console.WriteLine("Login Failed");
                Login();
            }
                
        }

        private static void SkipLogin() {
            connection = new FtpConnectionManager();
            if (connection.Validate())
            {
                logFile = "default" + ".log";
                ProcessInput();
            }
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
            File.AppendAllText(logFile, FormatLogText(cmd));
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

        private static string FormatLogText(String text)
        {
            DateTime localTime = DateTime.Now;
            string localTimeString = localTime.ToString(new CultureInfo("en-US"));

            return $"[{localTimeString}]   {text}{Environment.NewLine}";
        }
    }
}
