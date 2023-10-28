using System;
using ProgramServer.Domain.Common;

namespace ProgramServer.Domain.Surveys
{
    public class Survey : BaseEntity
    {
        public string Subject { get; set; }
        public string Question { get; set; }
    }
}

