using AutoMapper;
using ProgramServer.Application.Repository;
using ProgramServer.Domain.Users;
using ProgramServer.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using ProgramServer.Domain.Attendances;
using ProgramServer.Domain.Attandances;
using FluentValidation;

namespace ProgramServer.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Attendance> _attendanceRepository;
        private readonly IRepository<BluetoothCode> _bluetoothCodeRepository;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> userRepository,
            IRepository<Attendance> attendanceRepository,
            IRepository<BluetoothCode> bluetoothCodeRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _attendanceRepository = attendanceRepository;
            _bluetoothCodeRepository = bluetoothCodeRepository;
            _mapper = mapper;
        }

        public async Task Add(UserCreateModel userModel)
        {
            var validator = new UserCreateModelValidator();
            var result = validator.Validate(userModel);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            userModel.Password = " ";
            var user = _mapper.Map<User>(userModel);

            _userRepository.Add(user);

            await _userRepository.SaveAsync();
        }

        public async Task AddUsers(List<UserCreateModel> users)
        {
            foreach (var userModel in users)
            {
                var validator = new UserCreateModelValidator();
                var result = validator.Validate(userModel);

                if (!result.IsValid)
                    throw new ValidationException(result.Errors);

                userModel.Password = " ";
                var user = _mapper.Map<User>(userModel);

                var userExists = await _userRepository.Where(u => u.Email == userModel.Email).AnyAsync();

                if (userExists)
                {
                    continue;
                }
                else
                {
                    _userRepository.Add(user);
                    await _userRepository.SaveAsync();
                }
            }
        }


        public async Task<List<UserGetModel>> GetAll()
        {
            var allusers = await _userRepository.GetAll().ToListAsync();
            return _mapper.Map<List<UserGetModel>>(allusers);
        }

        public async Task Delete(int id)
        {
            await _userRepository.Delete(o => o.Id == id);
        }

    }
}

