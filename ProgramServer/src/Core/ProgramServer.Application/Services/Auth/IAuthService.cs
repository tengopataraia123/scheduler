namespace ProgramServer.Application.Services.Auth;

public interface IAuthService
{
    public Task<string> GenerateToken(string email);
}