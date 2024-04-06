using System;
namespace ProgramServer.Application.Exceptions
{
    public class CannotDeleteSubjectException : Exception
    {
        public CannotDeleteSubjectException(string message) : base(message)
        {
        }
    }
}

