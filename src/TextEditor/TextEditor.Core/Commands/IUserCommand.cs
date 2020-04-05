namespace TextEditor.Core.Commands
{
    public interface IUserCommand
    {
        string SucceedMessage { get; }
        string FailMessage { get; }
    }
}
