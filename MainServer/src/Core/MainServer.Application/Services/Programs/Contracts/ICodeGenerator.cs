using System;
using MainServer.Application.Services.Programs.Models;

namespace MainServer.Application.Services.Programs.Contracts
{
	public interface ICodeGenerator
	{
        string GenerateCodeForProgram(string name, string url);
    }
}

