using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramServer.Application.Services.RequestDecrypt.Models
{
    public class DecryptionKey
    {
        public string PrivateKey { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ValidUntilDate { get; set; }
    }
}
