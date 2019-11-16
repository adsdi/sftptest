using SftpTest;
using System;

namespace Org.ConsoleTest {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("ConsoleTest is startng...");

            var cmd = "";

            while (cmd != "stop") {
                Console.WriteLine();
                Console.WriteLine("Enter 'k' to test SFTP access using KellermanSoftware.NetSFTPLib");
                Console.WriteLine("Enter 'w' to test SFTP access using WinScp");
                Console.WriteLine("Enter 'stop' to terminate the program");

                cmd = Console.ReadLine().ToLower();

                switch (cmd) {
                    case "k":
                        Console.WriteLine();
                        Console.WriteLine("Before call to KellermanSoftware.NetSFTPLib...");
                        Console.WriteLine(SftpTestFunctionEntry.RunFromConsole(cmd));
                        Console.WriteLine("After call to KellermanSoftware.NetSFTPLib...");
                        break;

                    case "w":
                        Console.WriteLine();
                        Console.WriteLine("Before call to WinScp...");
                        Console.WriteLine(SftpTestFunctionEntry.RunFromConsole(cmd));
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
    }
}
