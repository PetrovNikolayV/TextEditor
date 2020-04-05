using System;
using System.IO;

namespace TextEditor.Core
{
    public class SourceTextLine :ITextLine
    {
        private readonly Stream _sourceStream;
        private readonly long _position;
        private readonly int _length;

        public SourceTextLine(Stream sourceStream, long position, int length)
        {
            _sourceStream = sourceStream;
            _position = position;
            _length = length;
        }

        public byte[] GetData()
        {
            if (_sourceStream.Position > _position)
            {
                throw new Exception("Current stream position is bigger than requested position");
            }

            long needToSkip = _position - _sourceStream.Position;
            _sourceStream.Seek(needToSkip, SeekOrigin.Current);
            byte[] result = new byte[_length];
            _sourceStream.Read(result,0, _length);
            return result;
        }
    }
}
