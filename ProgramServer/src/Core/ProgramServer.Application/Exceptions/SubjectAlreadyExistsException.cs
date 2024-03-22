using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramServer.Application.Exceptions
{
    public class SubjectAlreadyExistsException : ApplicationException
    {
        public SubjectAlreadyExistsException(string subjectCode)
            : base($"საგანი კოდით {subjectCode} უკვე არსებობს")
        {

        }
    }
}
