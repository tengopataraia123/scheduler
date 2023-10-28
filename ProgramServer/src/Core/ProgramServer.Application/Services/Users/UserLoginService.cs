using Microsoft.EntityFrameworkCore;
using ProgramServer.Application.Auth.Common;
using ProgramServer.Application.DTOs;
using ProgramServer.Application.Exceptions;
using ProgramServer.Application.Models.Login;
using ProgramServer.Application.Repository;
using ProgramServer.Domain.Users;

namespace ProgramServer.Application.Services.Users
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
            var user = await _userRepository.Where(x => x.Email.Equals(model.Mail))
            .Include(o => o.Role).FirstOrDefaultAsync();

            if (user is null)
                throw new NotFoundException(nameof(User),model.Mail);

            if (!_tokenService.Verify(model.Password, user.Password))
                throw new UnauthorizedAccessException("არასწორი პაროლი");

            var systemUser = _tokenService.BuildToken(new SystemUserModel()
            {
                Id = user.Id,
                Name = user.FirstName,
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

            var userEntity = await _userRepository.Where(x => x.Email.Equals(userRegistartion.Mail)).FirstOrDefaultAsync();
            if (userEntity != null)
                throw new AlreadyExistsException(nameof(User), userRegistartion.Mail);

            var user = new User()
            {
                FirstName = userRegistartion.FirstName,
                LastName = userRegistartion.LastName,
                Email = userRegistartion.Mail,
                Password = _tokenService.Hash(userRegistartion.Password),
                RoleId = 5,

            };
            
            _userRepository.Add(user);
            await _userRepository.SaveAsync();
        }

    }
}
