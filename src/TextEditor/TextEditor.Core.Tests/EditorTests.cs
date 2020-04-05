using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TextEditor.Core.Tests
{
    [TestClass]
    public class EditorTests
    {
        [TestMethod]
        public void TestEditCommands()
        {
            MemoryStream sourceStream = new MemoryStream();
            StreamWriter sourceWriter = new StreamWriter(sourceStream);
            sourceWriter.WriteLine("Line0");
            sourceWriter.WriteLine("Line1");
            sourceWriter.WriteLine("Line2");
            sourceWriter.Flush();
            sourceStream.Position = 0;
            MemoryStream operStream = new MemoryStream();

            Editor editor = new Editor(sourceStream,operStream);
            editor.Read();

            editor.Do(CommandParser.Parse("fsdfsdfs"));
            editor.Do(CommandParser.Parse("ins 0 NewLine1"));
            editor.Do(CommandParser.Parse("ins 2 NewLine2"));
            editor.Do(CommandParser.Parse("ins 5 NewLine3"));
            editor.Do(CommandParser.Parse("del 4"));
            editor.Do(CommandParser.Parse("save"));


            MemoryStream resultStream = new MemoryStream();
            StreamWriter resultWriter = new StreamWriter(resultStream);
            resultWriter.WriteLine("NewLine1");
            resultWriter.WriteLine("Line0");
            resultWriter.WriteLine("NewLine2");
            resultWriter.WriteLine("Line1");
            resultWriter.Write("NewLine3");
            resultWriter.Flush();
            resultStream.Position = 0;

            byte[] source = sourceStream.ToArray();
            byte[] result = resultStream.ToArray();

            sourceStream.Position = 0;
            resultStream.Position = 0;
            Assert.IsTrue(source.SequenceEqual(result));
        }
    }
}
