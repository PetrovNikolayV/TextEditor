using System;
using System.Collections.Generic;
using System.IO;

namespace TextEditor.Core
{
    public class TextLinesReader
    {
        private readonly Stream _stream;

        public TextLinesReader(Stream stream)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public LinkedList<StreamTextLine> ReadLines()
        {
            LinkedList<StreamTextLine> result = new LinkedList<StreamTextLine>();
            byte[] buffer = new byte[100000];

            long position = 0;
            int offset = 0;
            while (true)
            {
                int count = _stream.Read(buffer, 0, buffer.Length);
                if (count == 0)
                {
                    if (offset != 0)
                    {
                        StreamTextLine line = new StreamTextLine(_stream, position, offset);
                        result.AddLast(line);
                    }
                    break;
                }

                for (int i = 0; i < count; i++)
                {
                    offset++;
                    if (buffer[i] == 0x0A)
                    {
                        StreamTextLine line = new StreamTextLine(_stream,position,offset-1);
                        position += offset;
                        offset = 0;
                        result.AddLast(line);
                    }
                }
            }
            return result;
        }
    }
}
