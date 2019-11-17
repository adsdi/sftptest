using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.SftpTest.Shared;
using SftpTest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Org.ConsoleTest {
    class Program {
        private static IConfigurationRoot config;
        private static string appDirectory;
        private static ILogger log;

        static void Main(string[] args) {
            Console.WriteLine("ConsoleTest is startng...");
            config = loadConfig();
            var loggerFactory = (ILoggerFactory)new LoggerFactory();
            log = loggerFactory.CreateLogger("Main");
            log.LogInformation("Testing the logging to the console.");

            var cmd = "";

            while (cmd != "stop") {
                Console.WriteLine();
                Console.WriteLine("Enter 'k' to test SFTP access using KellermanSoftware.NetSFTPLib");
                Console.WriteLine("Enter 'w' to test SFTP access using WinScp");
                Console.WriteLine("Enter 'stop' to terminate the program");

                cmd = Console.ReadLine().ToLower().Trim();
                var sftpParms = getSftpParms(cmd);

                switch (cmd) {
                    case "k":
                        Console.WriteLine();
                        Console.WriteLine("Before call to KellermanSoftware.NetSFTPLib...");
                        Console.WriteLine();
                        var kellermanResult = SftpTestFunctionEntry.RunFromConsole(log, sftpParms).Result;
                        Console.WriteLine(kellermanResult.Report);
                        Console.WriteLine("After call to KellermanSoftware.NetSFTPLib...");
                        break;

                    case "w":
                        Console.WriteLine();
                        Console.WriteLine("Before call to WinScp...");;
                        Console.WriteLine();
                        var winscpResult = SftpTestFunctionEntry.RunFromConsole(log, sftpParms).Result;
                        Console.WriteLine(winscpResult.Report);
                        Console.WriteLine("After call to WinScp...");
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine($"*** The command '{cmd}' is invalid ***");
                        break;
                }

            }

            Console.WriteLine("ConsoleTest is ending.");
            Console.WriteLine("press any key to terminate.");
            Console.ReadLine();
        }

        private static IConfigurationRoot loadConfig() {
            try {
                appDirectory = Directory.GetCurrentDirectory();

                if (appDirectory.ContainsIn(@"bin\debug,bin\release")) {
                    int pos = appDirectory.IndexOf(@"\bin\");
                    appDirectory = appDirectory.Substring(0, pos);
                }

                config = new ConfigurationBuilder()
                    .SetBasePath(appDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                return config;
            }
            catch (Exception ex) {
                throw new Exception("An exception occurred while attempting to load the configurations.", ex);
            }
        }

        private static SftpParms getSftpParms(string cmd) {
            var sftpParms = new SftpParms();
            sftpParms.SftpLibrary = cmd == "k" ? "Kellerman" : "WinScp";
            sftpParms.SftpUrl = config[$"SftpUrl"];
            sftpParms.SftpHostFingerprint = config[$"SftpHostFingerprint"];
            sftpParms.SftpUserName = config[$"SftpUserName"];
            sftpParms.SftpPassword = config[$"SftpPassword"];
            sftpParms.SftpKellermanUser = config[$"SftpKellermanUser"];
            sftpParms.SftpKellermanLicense = config[$"SftpKellermanLicense"];
            sftpParms.WinScpBinDirectory = Environment.CurrentDirectory;
            return sftpParms;
        }
    }
}
