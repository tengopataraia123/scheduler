using System;
using AutoMapper;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
using MainServer.Application.Services.Programs.Contracts;
using MainServer.Domain.Programs;
using MainServer.Application.Repository;
using MainServer.Application.Services.Programs.Models;
using MainServer.Application.Exceptions;
using MainServer.Application.Common.Models;
using MainServer.Application.Services.Users.Contracts;
using System.Security.Cryptography;

namespace MainServer.Application.Services.Programs
{
    public class ProgramService : IProgramService
    {
        private readonly IRepository<ProgramEntity> _programRepository;
        private readonly IRepository<ProgramEncryptionKey> _encryptionKeyRepository;
        private readonly IUserService _userService;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IMapper _mapper;

        public ProgramService(IRepository<ProgramEntity> programRepository,
            IRepository<ProgramEncryptionKey> encryptionKeyRepository,
            IMapper mapper,
            IUserService userService,
            ICodeGenerator codeGenerator)
        {
            _programRepository = programRepository;
            _userService = userService;
            _mapper = mapper;
            _encryptionKeyRepository = encryptionKeyRepository;
            _codeGenerator = codeGenerator;
        }

        public async Task<ProgramModel> Find(int id)
        {
            var program = await _programRepository.GetByIdAsync(id);
            if (program == null)
                throw new NotFoundException(nameof(ProgramEntity), id);
            var mappedProgram = _mapper.Map<ProgramModel>(program);
            return mappedProgram;
        }

        public async Task<List<ProgramModel>> GetAll()
        {
            var allprograms = await _programRepository.GetAllAsync();
            return _mapper.Map<List<ProgramModel>>(allprograms);
        }

        public async Task Create(ProgramCreateModel programModel,int userId)
        {
            var validator = new ProgramCreateModelValidator();
            var result = validator.Validate(programModel);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var program = _mapper.Map<ProgramEntity>(programModel);

            program.IsBlocked = false;
            program.IsActive = false;
            program.UserId = userId;
            program.Code = string.Empty;

            await _programRepository.AddAsync(program);
        }

        //public async Task Update(int id, ProgramModel programModel, UserInfo userInfo)
        //{
        //    var validator = new ProgramValidator();
        //    var result = validator.Validate(programModel);
        //    if (!result.IsValid)
        //        throw new ValidationException(result.Errors);

        //    var program = new ProgramEntity();
        //    if (userInfo.RoleName.Equals("User"))
        //    {
        //        var user = await _userService.GetUserById(userInfo.Id);
        //        if (user.IsBlocked)
        //            throw new BadRequestException("The program can not be updated, user is blocked");

        //        program = await _programRepository.Find(x => x.Id == id && x.UserId == userInfo.Id);
        //        if (program.IsBlocked)
        //            throw new BadRequestException("the program can not be updated at this moment");
        //    }
        //    else
        //        program = await _programRepository.GetByIdAsync(id);


        //    program.Name = programModel.Name;
        //    program.Url = programModel.Url;
        //    //program.Code = _codeGenerator.GenerateCodeForProgram(programModel.Name, programModel.Url);
        //    program.IsActive = false;

        //    await _programRepository.UpdateAsync(program);
        //}

        public async Task Activate(int Id, UserInfo userInfo)
        {
            var validator = new ProgramValidator();
            var program = await _programRepository.GetByIdAsync(Id);
            if (program == null)
                throw new NotFoundException(nameof(ProgramEntity), Id);

            if (program.IsBlocked)
                throw new BadRequestException("program is blocked");

            var user = await _userService.GetUserById(userInfo.Id);
            if (userInfo.RoleName.Equals("Admin"))
            {
                program.IsActive = true;
                program.Code = _codeGenerator.GenerateCodeForProgram(program.Name, program.Url);
            }
            _mapper.Map<ProgramModel>(program);


            if (user.IsBlocked)
                throw new BadRequestException("user is blocked");

            //if (!program.HasBeenActivated)
            //    throw new BadRequestException("program can not be activated, untill admin activates it first");

            _mapper.Map<ProgramModel>(program);
            await _programRepository.UpdateAsync(program);
        }

        public async Task Deactivate(int id, UserInfo userInfo)
        {
            var validator = new ProgramValidator();
            var program = await _programRepository.GetByIdAsync(id);
            if (program == null)
                throw new NotFoundException(nameof(ProgramEntity), id);

            if (program.IsBlocked)
                throw new BadRequestException("program is blocked");

            var user = await _userService.GetUserById(userInfo.Id);
            if (userInfo.RoleName.Equals("Admin"))
                program.IsActive = false;

            if (user.IsBlocked)
                throw new BadRequestException("user is blocked");

            //if (!program.HasBeenActivated)
            //    throw new BadRequestException("program can not be deactvated, untill admin activates it first");

            await _programRepository.UpdateAsync(program);
        }

        public async Task Block(int id)
        {
            var validator = new ProgramValidator();
            var program = await _programRepository.GetByIdAsync(id);
            if (program == null)
                throw new NotFoundException(nameof(ProgramEntity), id);

            program.IsActive = false;
            program.IsBlocked = true;

            await _programRepository.UpdateAsync(program);
        }

        public async Task Unblock(int id)
        {
            var validator = new ProgramValidator();
            var program = await _programRepository.GetByIdAsync(id);
            if (program == null)
                throw new NotFoundException(nameof(ProgramEntity), id);

            program.IsActive = false;
            program.IsBlocked = false;

            await _programRepository.UpdateAsync(program);
        }

        public async Task<ProgramPrivateKeyOutDTO> GenerateNewKeyPair(int programId)
        {
            var program = await _programRepository.Find(o=>o.Id == programId);
            if(program == null)
                throw new NotFoundException(nameof(ProgramEntity), programId);

            var cryptoService = new RSACryptoServiceProvider(1024);

            var privateKey = cryptoService.ExportRSAPrivateKeyPem();
            var publicKey = cryptoService.ExportRSAPublicKeyPem();

            var keyPair = new ProgramEncryptionKey
            {
                CreateDate = DateTime.Now,
                ProgramId = programId,
                PrivateKey = privateKey,
                PublicKey = publicKey
            };

            await _encryptionKeyRepository.AddAsync(keyPair);

            return new ProgramPrivateKeyOutDTO
            {
                CreateDate = DateTime.Now,
                ValidUntilDate = DateTime.Now.AddDays(1),
                PrivateKey = privateKey,
            };

        }

        public async Task<ProgramPrivateKeyOutDTO> GetPrivateKey(int programId)
        {
            var programKeyPair = await _encryptionKeyRepository.Find(o => o.ProgramId == programId &&
                        o.CreateDate >= DateTime.Now.AddDays(-1) &&
                        o.Program.IsBlocked == false &&
                        o.Program.IsActive == true,"Program");

            if (programKeyPair == null)
                throw new NotFoundException(nameof(ProgramEncryptionKey), programId);

            return new ProgramPrivateKeyOutDTO
            {
                CreateDate = programKeyPair.CreateDate,
                ValidUntilDate = programKeyPair.CreateDate.AddDays(1),
                PrivateKey = programKeyPair.PrivateKey,
            };
        }

        public async Task<ProgramPublicKeyOutDTO> GetPublicKey(int programId)
        {
            var programKeyPair = await _encryptionKeyRepository.Find(o => o.ProgramId == programId &&
                        o.CreateDate >= DateTime.Now.AddDays(-1) &&
                        o.Program.IsBlocked == false &&
                        o.Program.IsActive == true, "Program");

            if (programKeyPair == null)
                throw new NotFoundException(nameof(ProgramEncryptionKey), programId);

            return new ProgramPublicKeyOutDTO
            {
                CreateDate = programKeyPair.CreateDate,
                ValidUntilDate = programKeyPair.CreateDate.AddDays(1),
                PublicKey = programKeyPair.PublicKey,
            };
        }

        public async Task<string> GetProgramUrl(string code)
        {
            var program = await _programRepository.Find(p=>p.Code == code);

            if(program.Url == null)
            {
                throw new NotFoundException(nameof(ProgramEntity), code);
            }

            return program.Url;
        }
    }
}

