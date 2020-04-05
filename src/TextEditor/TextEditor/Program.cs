using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TextEditor.Core;
using TextEditor.Core.Commands;

namespace TextEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = args.FirstOrDefault();
            
            if (string.IsNullOrEmpty(fileName))
            {
                Console.WriteLine("You should specify file name.");
                return;
            }

            var saveOperationFileName = fileName + ".tmp";

            if (!OpenStreams(fileName, saveOperationFileName, 
                out FileStream sourceStream, out FileStream saveOperationsStream))
            {
                return;
            }

            var editor = new Editor(sourceStream, saveOperationsStream);
            if (!ReadFile(editor, fileName))
            {
                sourceStream.Dispose();
                saveOperationsStream.Dispose();
                TryDeleteFile(saveOperationFileName);
                return;
            }
           
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

            sourceStream.Dispose();
            saveOperationsStream.Dispose();
            TryDeleteFile(saveOperationFileName);
        }

        private static void TryDeleteFile(string fileName)
        {
            try
            {
                File.Delete(fileName);
            }
            catch
            {
                Console.WriteLine($"Unable to delete file {fileName}");
            }
        }

        private static bool OpenStreams(
            string fileName, 
            string saveOperationFile, 
            out FileStream sourceStream, 
            out FileStream saveOperationsStream)
        {
            sourceStream = saveOperationsStream = null;
            try
            {
                sourceStream = new FileStream(fileName, FileMode.Open);
            }
            catch
            {
                Console.WriteLine($"Unable to open file {fileName}");
                return false;
            }

            try
            {
                saveOperationsStream = new FileStream(saveOperationFile, FileMode.Create);
            }
            catch
            {
                Console.WriteLine($"Unable to create file {saveOperationFile} for saving");
                return false;
            }

            return true;
        }

        private static bool ReadFile(Editor editor, string fileName)
        {
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                Console.WriteLine($"Start loading file {fileName}...");
                editor.Read();
                Console.WriteLine($"File was read in {watch.ElapsedMilliseconds} milliseconds");
                watch.Stop();
            }
            catch
            {
                Console.WriteLine($"Unable to read file {fileName}");
                return false;
            }

            return true;
        }
    }

    
}
