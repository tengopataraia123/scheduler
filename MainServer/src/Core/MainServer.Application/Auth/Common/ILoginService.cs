using MainServer.Application.Models.Login;

namespace MainServer.Application.Auth.Common
{
    public interface ILoginService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
    }
}

