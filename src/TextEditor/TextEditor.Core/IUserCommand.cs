namespace TextEditor.Core
{
    public interface IUserCommand
    {
        string SucceedMessage { get; }
        string FailMessage { get; }
    }
}
