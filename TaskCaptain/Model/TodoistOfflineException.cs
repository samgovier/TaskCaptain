using System;
using System.Runtime.Serialization;

namespace TaskCaptain
{
    internal class TodoistOfflineException : Exception
    {
        private string _message;

        public override string Message
        {
            get
            {
                return _message;
            }
        }

        public TodoistOfflineException()
        {
            _message = "Todoist Project or Task is offline.";
        }

        public TodoistOfflineException(string message) : base(message)
        {
            _message = message;
        }
    }
}