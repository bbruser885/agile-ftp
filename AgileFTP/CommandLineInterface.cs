using System;
using System.IO;
using FtpConnection;
using System.Timers;
using System.Globalization;
using System.Collections.Generic;

namespace AgileFTP {
    public static class CommandLineInterface {

        public static FtpConnectionManager connection;
        public static bool running;
        public static Timer timeoutTimer;
        public static string userName;
        public static string logFile;

        public static void Start() {
            timeoutTimer = new Timer();
            timeoutTimer.Interval = 60000; //TODO decide on the timeout interval temporarily set to one minute

            timeoutTimer.Elapsed += OnTimeoutEvent;
            timeoutTimer.Enabled = true;
            ListConnections();
        }

        private static void ListConnections() {
            List<SavedConnection> connections = LoadConnections();
            Console.WriteLine("Select a Connection:");
            for (int i = 0; i < connections.Count; i++) {
                Console.WriteLine("  ["+i+"] " + connections[i].username + "@" + connections[i].hostname);
            }
            Console.WriteLine("  [" + connections.Count + "] New Connection...");
            String h = Console.ReadLine();
            int option = Int32.Parse(h);
            if (option == connections.Count) {
                NewConnection();
            } else {
                Login(connections[option].username, connections[option].hostname);
            }
        }

        private static void NewConnection() {
            Console.Write("Enter hostname:");
            String h = Console.ReadLine();
            if (h == "?") {
                SkipLogin();
                return;
            }
            Console.Write("Enter username:");
            String u = Console.ReadLine();
            Login(u,h, true);
        }

        private static void Login(string u, string h, bool shouldSaveIfSuccessful = false) {
            logFile = $"{u}.log";
            Console.Write("Enter password:");
            String p = Console.ReadLine();
            connection = new FtpConnectionManager(u, p, h);
            if (connection.Validate()) {
                if (shouldSaveIfSuccessful)
                    SaveConnection(new SavedConnection(h,u));
                ProcessInput();
            } else {
                Console.WriteLine("Login Failed");
                NewConnection();
            }
                
        }

        private static void SkipLogin() {
            connection = new FtpConnectionManager();
            if (connection.Validate())
            {
                logFile = "default.log";
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

        private static void SaveConnection(SavedConnection c) {
            try {
                StreamWriter sw = new StreamWriter("connections.data");
                sw.WriteLine(c.username + "@" + c.hostname);
                sw.Close();
            }  catch (Exception e) {

            }
        }

        private static List<SavedConnection> LoadConnections() {
            List<SavedConnection> con = new List<SavedConnection>();
            try {
                StreamReader sr = new StreamReader("connections.data");
                string l;
                while ((l = sr.ReadLine()) != null) {
                    string[] split = l.Split("@");
                    SavedConnection sc = new SavedConnection(split[1], split[0]);
                    con.Add(sc);
                }
                sr.Close();
            } catch (Exception e) {
                
            }
            return con;
        }
    }

    public struct SavedConnection {
        public string hostname;
        public string username;
        public SavedConnection(string h, string u) {
            hostname = h;
            username = u;
        }
    }
        
}
