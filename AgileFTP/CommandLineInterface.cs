using System;
using System.IO;
using FtpConnection;
using System.Timers;
using System.Globalization;
using AgileFTP.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AgileFTP {
    public static class CommandLineInterface {

        public static FtpConnectionManager connection;
        public static bool running;
        public static Timer timeoutTimer;
        public static string userName;
        public static string Host;
        public static string Password;
        public static string logFile;
        public static string connectionsFile = "connections.txt";

        public static void Start() {
            timeoutTimer = new Timer();
            timeoutTimer.Interval = 60000; //TODO decide on the timeout interval temporarily set to one minute

            timeoutTimer.Elapsed += OnTimeoutEvent;
            timeoutTimer.Enabled = true;

            bool decision = AskYesNoQuestion("load saved connection? y/n ");

            if (!decision)
            {
                Login();
                return;
            }

            if (File.Exists(connectionsFile))
            {
                Connections fileList;
                try
                {
                    fileList = JsonConvert.DeserializeObject<Connections>(File.ReadAllText(connectionsFile));
                }
                catch (Exception ex)
                {
                    throw new FileLoadException("failed ot load connections", ex);
                }
                LoginWithSavedConnection(fileList);
            }
            else
            {
                Console.WriteLine("no saved connections, login required");
                Login();
            }
        }

        private static void LoginWithSavedConnection(Connections fileList)
        {
            List<string> connectionNames = new List<string>();
            Console.WriteLine("choose connection:");
            foreach (var c in fileList.ConnectionList)
            {
                connectionNames.Add(c.Name);
                Console.WriteLine(c.Name);
            }
            var connNameChoice = Console.ReadLine();
            foreach (var c in fileList.ConnectionList)
            {
                if (connNameChoice == c.Name)
                {
                    connection = new FtpConnectionManager(c.User, c.Password, c.Host);
                    if (connection.Validate())
                    {
                        logFile = $"{c.User}.log";
                        ProcessInput();
                        Environment.Exit(1);

                    }
                }
            }
            Console.WriteLine("Invalid Name");
            LoginWithSavedConnection(fileList);
        }

        private static void Login() {
            Console.Write("Enter hostname:");
            String h = Console.ReadLine();
            Host = h;
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
                var choice = AskYesNoQuestion("Save Login? y/n: ");
                if (choice)
                {
                    SaveLogin();
                }
                ProcessInput();
            }
            else {
                Console.WriteLine("Login Failed");
                Login();
            }
                
        }

        private static void SaveLogin()
        {
            Console.Write("Save connection as: ");
            String connectionName = Console.ReadLine();
            Connection conn = new Connection();
            conn.User = userName;
            conn.Host = Host;
            conn.Password = Password;
            conn.Name = connectionName;

            if (!File.Exists(connectionsFile))
            {
                Connections connections = new Connections();
                connections.ConnectionList = new List<Connection>();
                connections.ConnectionList.Add(conn);
                using (StreamWriter file = File.CreateText(connectionsFile))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, connections);
                }
            }
            else
            {
                Connections fileList;
                try
                {
                    fileList = JsonConvert.DeserializeObject<Connections>(File.ReadAllText(connectionsFile));
                }
                catch (Exception ex)
                {
                    throw new FileLoadException("failed to load connection", ex);
                }
                List<string> connectionNames = new List<string>();
                foreach (var c in fileList.ConnectionList)
                {
                    connectionNames.Add(c.Name);
                }
                if (connectionNames.Contains(conn.Name))
                {
                    Console.WriteLine("Connection Name In Use");
                    SaveLogin();
                }
                else
                {
                    fileList.ConnectionList?.Add(conn);
                    using (StreamWriter file = File.CreateText(connectionsFile))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, fileList);
                    }
                }
            }
        }

        private static void SkipLogin() {
            connection = new FtpConnectionManager();
            if (connection.Validate())
            {
                logFile = "defaut.log";
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

        private static bool AskYesNoQuestion(string question)
        {
            Console.Write(question);
            var choice = Console.ReadLine();
            if (choice == "y" || choice == "n")
            {
                return choice == "y";
            }
            return AskYesNoQuestion(question);
        }
    }
}
