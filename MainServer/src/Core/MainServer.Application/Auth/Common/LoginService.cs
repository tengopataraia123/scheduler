using MainServer.Application.Models.Login;
using MainServer.Domain.Users;

namespace MainServer.Application.Auth.Common
{
    public class LoginService : ILoginService
    {
        private readonly ITokenService _tokenService;

        public LoginService(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = new User();

            if (user == null) return null;

            return new AuthenticateResponse(new SystemUserModel());
        }
    }
}


