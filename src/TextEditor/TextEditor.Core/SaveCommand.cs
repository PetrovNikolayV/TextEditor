﻿namespace TextEditor.Core
{
    public class SaveCommand : IUserCommand
    {
        public string SucceedMessage => "File has been saved";
        public string FailMessage => $"Unable to save file";
    }
}