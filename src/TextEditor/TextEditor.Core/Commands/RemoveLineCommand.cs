namespace TextEditor.Core.Commands
{
    class RemoveLineCommand : IUserCommand
    {
        public int Position { get; set; }
        public string SucceedMessage => $"Line number {Position} has been successfully removed";
        public string FailMessage => $"Unable to delete line number {Position}";
    }
}