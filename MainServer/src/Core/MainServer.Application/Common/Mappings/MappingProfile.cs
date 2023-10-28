using AutoMapper;
using MainServer.Application.Services.Programs.Models;
using MainServer.Application.Services.Users.Models;
using MainServer.Domain.Programs;
using MainServer.Domain.Users;

namespace MainServer.Application.Common.Mappings
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<ProgramEntity, ProgramCreateModel>().ReverseMap();
            CreateMap<ProgramEntity, ProgramModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}

