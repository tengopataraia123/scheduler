using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramServer.Application.DTOs
{
    public class SubjectUserModel
    {
        public string SubjectCode { get; set; }
        public string UserEmail { get; set; }
        public int SubjectId { get; set; }
        public int UserId { get; set; }
    }
}
