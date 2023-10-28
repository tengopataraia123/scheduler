using AutoMapper;
using Newtonsoft.Json;
using ProgramServer.Domain.Users;
using ProgramServer.Domain.Attendances;
using ProgramServer.Domain.Events;
using ProgramServer.Domain.Roles;
using ProgramServer.Domain.Subjects;
using ProgramServer.Domain.SubjectUsers;
using ProgramServer.Domain.Surveys;
using ProgramServer.Application.DTOs;

namespace ProgramServer.Application.Common.Mappings
{
    public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<Attendance, AttendanceModel>().ReverseMap();
            CreateMap<Event, EventModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<Role, RoleModel>().ReverseMap();
            CreateMap<Subject, SubjectModel>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<DescriptionModel>(src.DescriptionStr)))
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.SubjectUsers))
                .ForMember(dest => dest.Events, opt => opt.MapFrom(src => src.Events))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
            CreateMap<SubjectUser, UserModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<Survey, SurveyModel>().ReverseMap();
            CreateMap<Response, ResponseModel>().ReverseMap();
            CreateMap<Event,EventCreateModel>().ReverseMap();

            CreateMap<SubjectCreateModel, Subject>()
                .ForMember(dest => dest.DescriptionStr, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Description)))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
            CreateMap<SubjectUserModel, SubjectUser>().ReverseMap();
        }
    }
}

