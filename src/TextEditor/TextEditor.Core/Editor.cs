using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TextEditor.Core.Commands;

namespace TextEditor.Core
{
    public class Editor
    {
        private readonly byte[] _nextLineSymbols = new byte[] {0x0D, 0x0A};
        private readonly Stream _sourceStream;
        private readonly Stream _saveOperationStream;
        
        private LinkedList<StreamTextLine> _lines;
        private int _count = -1;
        private bool _isChanged;
        
        public Editor(Stream sourceStream, Stream saveOperationStream)
        {
            _sourceStream = sourceStream ?? throw new ArgumentNullException(nameof(sourceStream));
            _saveOperationStream = saveOperationStream ?? throw new ArgumentNullException(nameof(saveOperationStream));
        }

        public void Read()
        {
            _lines = new TextLinesReader(_sourceStream).ReadLines();
            _count = _lines.Count;
        }

        public void Do(IUserCommand command)
        {
            switch (command)
            {
                case AddLineCommand cmd: AddLine(cmd.Text, cmd.Position); break;
                case RemoveLineCommand cmd: RemoveLine(cmd.Position); break;
                case SaveCommand _: Save(); break;
            }
        }

        private void Save()
        {
            if (!_isChanged)
            {
                return;
            }

            _sourceStream.Position = 0;
            _saveOperationStream.SetLength(0);

            var node = _lines.First;
            while (node != null)
            {
                byte[] toWrite = node.Value.GetData();
                long pos = _saveOperationStream.Position;
                _saveOperationStream.Write(toWrite, 0, toWrite.Length);
                node.Value.SetPositionInStream(_sourceStream, pos, toWrite.Length);
                node = node.Next;
                if (node != null)
                {
                    _saveOperationStream.Write(_nextLineSymbols,0,_nextLineSymbols.Length);
                }
            }

            _sourceStream.Position = 0;
            _sourceStream.SetLength(0);
            _saveOperationStream.Position = 0;
            _saveOperationStream.CopyTo(_sourceStream);
            _sourceStream.Position = 0;
            _isChanged = false;
        }

        private void AddLine(string text, int position)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            StreamTextLine newLine = new StreamTextLine(bytes.ToArray());

            if (position == _count)
            {
                _lines.AddLast(newLine);
                _count++;
                _isChanged = true;
                return;
            }
            var line = GetNodeAtIndex(position);
            _lines.AddBefore(line, newLine);
            _count++;
            _isChanged = true;
        }

        private void RemoveLine(int position)
        {
            var line = GetNodeAtIndex(position);
            _lines.Remove(line);
            _count--;
            _isChanged = true;
        }

        private LinkedListNode<StreamTextLine> GetNodeAtIndex(int index)
        {
            if (index < 0 || index >= _count)
            {
                throw new ArgumentOutOfRangeException();
            }

            LinkedListNode<StreamTextLine> result = _lines.First;
            if (result == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            while (index > 0)
            {
                result = result.Next ?? throw new ArgumentOutOfRangeException();
                index--;
            }

            return result;
        }
    }
}
