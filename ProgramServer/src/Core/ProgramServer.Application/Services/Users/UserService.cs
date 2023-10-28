using System;
using AutoMapper;
using ProgramServer.Application.Repository;
using ProgramServer.Domain.Users;
using System.Linq.Expressions;
using ProgramServer.Application.Exceptions;
using System.Runtime.ConstrainedExecution;
using ProgramServer.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using ProgramServer.Domain.Attendances;
using ProgramServer.Domain.Attandances;

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

        public async Task<UserModel> FindUser(int id)
        {
            var user = await _userRepository.Where(o=>o.Id == id).FirstOrDefaultAsync();
            if (user == null)
                throw new NotFoundException(nameof(User), id);
            return _mapper.Map<UserModel>(user);
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            var allusers = await _userRepository.GetAll().ToListAsync();
            return _mapper.Map<List<UserModel>>(allusers);
        }

        public async Task DeleteUser(int id)
        {
            await _userRepository.Delete(o=>o.Id == id);
        }

    }
}

