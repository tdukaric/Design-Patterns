using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdukaric_zadaca_1
{

    class Program
    {
        /// <summary>
        /// Main user interface - switch-case type
        /// </summary>
        /// <param name="fileSystem">Object "FS" with file system</param>
        static void commands(IFS fileSystem)
        {
            string x = null;
            x = Console.ReadLine();
            x = x.Trim();

            int i;
            int j;
            string[] commands = null;

            commands = x.Split(' ');

            switch (commands[0].ToLower())
            {
                case "ls":
                case "dir":
                    if (commands.Length == 1)
                        Console.WriteLine(fileSystem.main.Show(0));
                    else
                        if (Int32.TryParse(commands[1], out i))
                        {
                            IComponent temp = fileSystem.main.FindComponent(i);
                            if (temp == null)
                            {
                                Console.WriteLine("Object doesn't exist!");
                            Program.commands(fileSystem);
                            }
                            Console.WriteLine(temp.Show(0));
                        }
                        else
                            Console.WriteLine("Wrong command, enter \"help\" for more info.");

                    break;

                case "copy":
                case "cp":
                    if (Int32.TryParse(commands[1], out i) && Int32.TryParse(commands[2], out j))
                        if (commands.Length == 4)
                            fileSystem.CopyComponent(i, j, commands[3]);
                        else
                            fileSystem.CopyComponent(i, j);
                    else
                        Console.WriteLine("Wrong command, enter \"help\" for more info.");
                    break;

                case "move":
                case "mv":
                    if (Int32.TryParse(commands[1], out i) && Int32.TryParse(commands[2], out j))
                        fileSystem.MoveComponent(i, j);
                    else
                        Console.WriteLine("Wrong command, enter \"help\" for more info.");
                    break;

                case "rm":
                    if (Int32.TryParse(commands[1], out i))
                        fileSystem.RemoveComponent(i);
                    else
                        Console.WriteLine("Wrong command, enter \"help\" for more info.");
                    break;

                case "rid":
                    if (Int32.TryParse(commands[1], out i))
                        Console.WriteLine(fileSystem.ShowReverse(i));
                    else
                        Console.WriteLine("Wrong command, enter \"help\" for more info.");
                    break;

                case "mkdir":
                    if (Int32.TryParse(commands[1], out i))
                        fileSystem.CreateComponentOnFS(i, commands[2], true);
                    else
                        Console.WriteLine("Wrong command, enter \"help\" for more info.");
                    break;

                case "touch":
                    if (Int32.TryParse(commands[1], out i))
                        fileSystem.CreateComponentOnFS(i, commands[2], false);
                    else
                        Console.WriteLine("Wrong command, enter \"help\" for more info.");
                    break;

                case "help":
                case "pomoc":
                    StringBuilder help = new StringBuilder();
                    help.AppendLine("DIR [from where]");
                    help.AppendLine("CP what where [name]");
                    help.AppendLine("MV what where [name]");
                    help.AppendLine("RM what");
                    help.AppendLine("RID what");
                    help.AppendLine("MKDIR where name");
                    help.AppendLine("TOUCH where name");
                    help.AppendLine("OPEN what");
                    Console.WriteLine(help.ToString());
                    break;

                case "open":
                case "otvori":
                    if (Int32.TryParse(commands[1], out i))
                        fileSystem.Open(i);
                    else
                        Console.WriteLine("Wrong command, enter \"help\" for more info.");
                    break;

                case "quit":
                    System.Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Wrong command, enter \"help\" for more info.");
                    break;

            }
            Console.WriteLine();
            Program.commands(fileSystem);
        }

        static void Main(string[] args)
        {
            string DS_TIP = Environment.GetEnvironmentVariable("DS_TIP");
            if (DS_TIP == null)
            {
                Console.WriteLine("Variable DS_TIP isn't defined.");
                return;
            }

            if (!(DS_TIP == "NTFS" || DS_TIP == "exFAT"))
            {
                Console.WriteLine("File system not supported!");
                return;
            }

            IFS FileSystem;

            if (DS_TIP == "NTFS")
            {
                FileSystem = NTFS.GetInstance(args[0], DS_TIP);
            }
            else
            {
                FileSystem = exFAT.GetInstance(args[0], DS_TIP);
            }
            Console.WriteLine(DS_TIP);
            
            FileSystem.PrintFS(FileSystem.main);
            Console.WriteLine("DS_TIP : " + FileSystem.DS_Type);
            commands(FileSystem);
        }
    }
}
