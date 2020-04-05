using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextEditor.Core.Commands;

namespace TextEditor.Core.Tests
{
    [TestClass]
    public class CommandParserTests
    {
        [TestMethod]
        public void TestInsert()
        {
            IUserCommand cmd = CommandParser.Parse("iNs 10888  test test test");
            AddLineCommand addLineCommand = cmd as AddLineCommand;
            Assert.IsNotNull(addLineCommand);
            Assert.AreEqual(10888, addLineCommand.Position);
            Assert.AreEqual(" test test test", addLineCommand.Text);
        }

        [TestMethod]
        public void TestRemove()
        {
            IUserCommand cmd = CommandParser.Parse("dEl 10888");
            RemoveLineCommand removeLineCommand = cmd as RemoveLineCommand;
            Assert.IsNotNull(removeLineCommand);
            Assert.AreEqual(10888, removeLineCommand.Position);
        }

        [TestMethod]
        public void TestSave()
        {
            IUserCommand cmd = CommandParser.Parse("sAve");
            Assert.IsInstanceOfType(cmd, typeof(SaveCommand));
        }


        [TestMethod]
        public void TestQuit()
        {
            IUserCommand cmd = CommandParser.Parse("QuiT");
            Assert.IsInstanceOfType(cmd, typeof(QuitCommand));
        }

        [TestMethod]
        public void TestUnknown()
        {
            IUserCommand cmd = CommandParser.Parse("QudsfgiT asd asdf asdf");
            Assert.IsNull(cmd);
        }

    }
}
