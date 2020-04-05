using System;
using System.Diagnostics;
using System.IO;
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
            }

            Console.ReadKey();
        }
    }
}
