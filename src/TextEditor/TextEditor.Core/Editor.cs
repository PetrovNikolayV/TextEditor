using System;
using System.Collections.Generic;
using System.IO;

namespace TextEditor.Core
{
    public class Editor : IDisposable
    {
        private readonly string _fileName;
        
        private LinkedList<ITextLine> _lines;
        private int _count;
        private Stream _sourceStream;

        public Editor(string fileName)
        {
            _fileName = fileName;
        }

        public void Open()
        {
            _sourceStream = new FileStream(_fileName, FileMode.Open);
            _lines = new TextLinesReader(_sourceStream).ReadLines();
            _count = _lines.Count;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void AddLine(string text, int position)
        {
            var line = GetNodeAtIndex(position);
            ITextLine newLine = new AddedTextLine(text);
            _lines.AddBefore(line, newLine);
            _count++;
        }

        public void RemoveLine(int position)
        {
            var line = GetNodeAtIndex(position);
            _lines.Remove(line);
            _count--;
        }

        private LinkedListNode<ITextLine> GetNodeAtIndex(int index)
        {
            if (index < 0 || index >= _count)
            {
                throw new ArgumentOutOfRangeException();
            }

            LinkedListNode<ITextLine> result = _lines.First;
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

        public void Dispose()
        {
            _sourceStream?.Dispose();
        }
    }
}
