using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainServer.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public static string ErrorMessage { get; private set; }
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : base(BuildErrorMessage(failures))
        {
            
        }

        private static string BuildErrorMessage(IEnumerable<ValidationFailure> failures)
        {
            var arr = failures.Select(x => $" {x.ErrorMessage}.");
            return "Validation failed: " + string.Join(string.Empty, arr);
        }
    }
}
