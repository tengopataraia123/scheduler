using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer.Application.Exceptions
{
    public class ProgramApiValidationException : ApplicationException
    {
        public ProgramApiValidationException()
            : base("Program doesn't meet required criteria")
        { }
    }
}
