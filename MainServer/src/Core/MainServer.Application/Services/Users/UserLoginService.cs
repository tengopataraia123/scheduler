using MainServer.Application.Auth.Common;
using MainServer.Application.Exceptions;
using MainServer.Application.Models.Login;
using MainServer.Application.Repository;
using MainServer.Application.Services.Users.Contracts;
using MainServer.Application.Services.Users.Models;
using MainServer.Domain.Users;

namespace MainServer.Application.Services.Users
{
    public class UserLoginService : IUserLoginService
    {
        private readonly IRepository<User> _userRepository;
        private readonly ITokenService _tokenService;

        public UserLoginService(IRepository<User> userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await _userRepository.Find(x => x.Mail.Equals(model.Mail),"Role");

            if (user is null)
                throw new NotFoundException(nameof(User),model.Mail);

            if (!_tokenService.Verify(model.Password, user.Password))
                throw new UnauthorizedAccessException("არასწორი პაროლი");

            var systemUser = _tokenService.BuildToken(new SystemUserModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = user.Role,
            });

            return new AuthenticateResponse(systemUser);
        }

        public async Task UserRegistration(UserRegistrationModel userRegistartion)
        {
            var validator = new UserRegistrationModelValidator();
            var result = validator.Validate(userRegistartion);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var userEntity = await _userRepository.Find(x => x.Mail.Equals(userRegistartion.Mail));
            if (userEntity != null)
                throw new AlreadyExistsException(nameof(User), userRegistartion.Mail);

            var user = new User()
            {
                FirstName = userRegistartion.FirstName,
                LastName = userRegistartion.LastName,
                Mail = userRegistartion.Mail,
                IsBlocked = false,
                UserName = userRegistartion.UserName,
                Password = _tokenService.Hash(userRegistartion.Password),
                RoleId = 2,
            };
            
            await _userRepository.AddAsync(user);
        }

    }
}
