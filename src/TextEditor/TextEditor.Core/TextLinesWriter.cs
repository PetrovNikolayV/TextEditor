using System;
using System.Collections.Generic;
using System.IO;

namespace TextEditor.Core
{
    public class TextLinesWriter
    {
        private readonly Stream _outputStream;

        public TextLinesWriter(Stream outputStream)
        {
            _outputStream = outputStream ?? throw new ArgumentNullException(nameof(outputStream));
        }

        public void WriteLines(ICollection<ITextLine> lines)
        {
            foreach (ITextLine line in lines)
            {
                byte[] bytes = line.GetData();
                _outputStream.Write(bytes,0,bytes.Length);
            }
        }
    }
}