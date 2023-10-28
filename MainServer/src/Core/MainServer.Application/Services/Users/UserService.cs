using System;
using AutoMapper;
using MainServer.Application.Exceptions;
using MainServer.Application.Repository;
using MainServer.Application.Services.Users.Contracts;
using MainServer.Application.Services.Users.Models;
using MainServer.Domain.Programs;
using MainServer.Domain.Users;

namespace MainServer.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task BlockUser(int userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user is null)
                throw new NotFoundException(nameof(User), userId);

            user.IsBlocked = true;
            await _repository.UpdateAsync(user);

        }

        public async Task UnBlockUser(int userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user is null)
                throw new NotFoundException(nameof(User), userId);

            user.IsBlocked = false;
            await _repository.UpdateAsync(user);

        }

        public async Task<UserModel> GetUserById(int userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user is null)
                throw new NotFoundException(nameof(User), userId);

            return _mapper.Map<UserModel>(user);
        }
    }
}

