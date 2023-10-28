using MainServer.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer.Domain.Programs
{
    public class ProgramEncryptionKey : BaseEntity
    {
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public DateTime CreateDate { get; set; }
        public int ProgramId { get; set; }
        public ProgramEntity Program { get; set; }
    }
}
