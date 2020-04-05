namespace TextEditor.Core
{
    public class AddLineCommand : IUserCommand
    {
        public int Position { get; set; }
        public string Text { get; set; }
        public string SucceedMessage => $"Line has been successfully added at position {Position}";
        public string FailMessage => $"Unable to add line at position {Position}";
    }
}