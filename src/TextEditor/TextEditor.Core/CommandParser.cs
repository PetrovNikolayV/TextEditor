using System;
using System.Runtime.InteropServices.WindowsRuntime;
using TextEditor.Core.Commands;

namespace TextEditor.Core
{
    public static class CommandParser
    {
        public static IUserCommand Parse(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                return null;
            }

            if (command.StartsWith("ins ", StringComparison.OrdinalIgnoreCase))
            {
                return CreateInsertCommand(command);
            }

            var words = command.Split(' ');

            var cmdType = words[0].ToLower();

            switch (cmdType)
            {
                case "del": return CreateRemoveCommand(words);
                case "save": return new SaveCommand();
                case "quit": return new QuitCommand();
                default: return null;
            }
        }

        private static AddLineCommand CreateInsertCommand(string command)
        {
            int index = 4;
            for (; index < command.Length; index++)
            {
                if (command[index] != ' ')
                {
                    continue;
                }
                break;
            }

            if (index >= command.Length)
            {
                return null;
            }

            string number = command.Substring(4, index - 3);
            if (!int.TryParse(number, out int pos))
            {
                return null;
            }

            string text = command.Substring(index + 1);

            return new AddLineCommand()
            {
                Position = pos,
                Text = text
            };
        }

        private static RemoveLineCommand CreateRemoveCommand(string[] words)
        {
            if (words.Length != 2 || !int.TryParse(words[1], out int pos))
            {
                return null;
            }

            return new RemoveLineCommand()
            {
                Position = pos
            };
        }
    }
}
