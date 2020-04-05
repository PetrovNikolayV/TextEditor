using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;

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

        public void Do(IUserCommand command)
        {
            switch (command)
            {
                case AddLineCommand cmd: AddLine(cmd.Text, cmd.Position); break;
                case RemoveLineCommand cmd: RemoveLine(cmd.Position); break;
                case SaveCommand _: Save(); break;
                case QuitCommand _: Dispose(); break;
            }
        }

        private void Save()
        {
            throw new NotImplementedException();
        }

        private void AddLine(string text, int position)
        {
            var line = GetNodeAtIndex(position);
            ITextLine newLine = new AddedTextLine(text);
            _lines.AddBefore(line, newLine);
            _count++;
        }

        private void RemoveLine(int position)
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
