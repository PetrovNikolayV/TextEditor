using System;
using System.IO;

namespace TextEditor.Core
{
    class StreamTextLine
    {
        private byte[] _data;
        private Stream _sourceStream;
        private long _position;
        private int _length;

        public StreamTextLine(Stream sourceStream, long position, int length)
        {
            _sourceStream = sourceStream;
            _position = position;
            _length = length;
        }

        public StreamTextLine(byte[] data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public void SetPositionInStream(Stream sourceStream, long position, int length)
        {
            _sourceStream = sourceStream;
            _position = position;
            _length = length;
            _data = null;
        }

        public byte[] GetData()
        {
            if (_data != null)
                return _data;

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
