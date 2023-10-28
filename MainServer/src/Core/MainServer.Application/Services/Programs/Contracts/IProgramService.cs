using System;
using MainServer.Application.Common.Models;
using MainServer.Application.Services.Programs.Models;

namespace MainServer.Application.Services.Programs.Contracts
{
	public interface IProgramService
	{
        Task<ProgramModel> Find(int id);
        Task<List<ProgramModel>> GetAll();
        Task Create(ProgramCreateModel programModel,int userId);
        Task Activate(int id, UserInfo userInfo);
        Task Deactivate(int id, UserInfo userInfo);
        Task Block(int id);
        Task Unblock(int id);
        Task<ProgramPrivateKeyOutDTO> GenerateNewKeyPair(int programId);
        Task<ProgramPrivateKeyOutDTO> GetPrivateKey(int programId);
        Task<ProgramPublicKeyOutDTO> GetPublicKey(int programId);
        Task<string> GetProgramUrl(string code);
    }
}
