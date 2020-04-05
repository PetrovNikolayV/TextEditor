using System.Text;

namespace TextEditor.Core
{
    public class AddedTextLine : ITextLine
    {
        private readonly byte[] _bytes;
        public AddedTextLine(string text)
        {
            _bytes = Encoding.UTF8.GetBytes(text+"\n");
        }

        public byte[] GetData()
        {
            return _bytes;
        }
    }
}