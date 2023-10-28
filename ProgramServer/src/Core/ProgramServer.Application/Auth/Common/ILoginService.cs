using ProgramServer.Application.Models.Login;

namespace ProgramServer.Application.Auth.Common
{
    public interface ILoginService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
    }
}

