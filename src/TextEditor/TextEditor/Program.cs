using System;
using System.Diagnostics;
using System.Linq;
using TextEditor.Core;

namespace TextEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = args.FirstOrDefault();

            if (string.IsNullOrEmpty(fileName))
            {
                Console.WriteLine("You should specify file name.");
                return;
            }
            
            Stopwatch watch = new Stopwatch();

            using (var editor = new Editor(fileName))
            {
                try
                {
                    watch.Start();
                    Console.WriteLine($"Start loading file {fileName}...");
                    editor.Open();
                    Console.WriteLine($"File was read in {watch.ElapsedMilliseconds} milliseconds");
                }
                catch
                {
                    Console.WriteLine($"Unable to open file {fileName}");
                }
                watch.Stop();

                while (true)
                {
                    Console.Write("Enter next command: ");
                    var command = Console.ReadLine();
                    IUserCommand cmd = CommandParser.Parse(command);
                    if (cmd == null)
                    {
                        Console.WriteLine("Unknown command. Try again...");
                        continue;
                    }

                    if (cmd is QuitCommand)
                    {
                        break;
                    }

                    try
                    {
                        editor.Do(cmd);
                        Console.WriteLine(cmd.SucceedMessage);
                    }
                    catch
                    {
                        Console.WriteLine(cmd.FailMessage);
                    }
                    
                }
            }
        }
    }
}
