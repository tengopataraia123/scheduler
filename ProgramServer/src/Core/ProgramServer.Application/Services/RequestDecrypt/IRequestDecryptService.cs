using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramServer.Application.Services.RequestDecrypt
{
    public interface IRequestDecryptService
    {
        Task RenewPrivateKey();

        Task<string> GetDecryptionKey();
    }
}
