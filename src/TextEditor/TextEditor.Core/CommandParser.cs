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

            var words = command.Split(' ');

            var cmdType = words[0].ToLower();

            switch (cmdType)
            {
                case "ins": return CreateInsertCommand(words);
                case "del": return CreateRemoveCommand(words);
                case "save": return new SaveCommand();
                case "quit": return new QuitCommand();
                default: return null;
            }
        }

        private static AddLineCommand CreateInsertCommand(string[] words)
        {
            if (words.Length != 3 || !int.TryParse(words[1], out int pos))
            {
                return null;
            }

            return new AddLineCommand()
            {
                Position = pos,
                Text = words[2]
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
