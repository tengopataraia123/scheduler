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
            CreateMap<SubjectUser, UserCreateModel>().ReverseMap();
            CreateMap<SubjectUserModel, SubjectUser>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId));

            CreateMap<User, UserCreateModel>().ReverseMap();
            CreateMap<User, UserGetModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<Survey, SurveyModel>().ReverseMap();
            CreateMap<Response, ResponseModel>().ReverseMap();
            CreateMap<Event, EventCreateModel>().ReverseMap();
            CreateMap<Event, EventGetModel>()
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap(); 


            CreateMap<SubjectCreateModel, Subject>()
                .ForMember(dest => dest.DescriptionStr, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Description)))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
            CreateMap<Subject, SubjectGetModel>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src =>
                    JsonConvert.DeserializeObject<DescriptionModel>(src.DescriptionStr) ?? new DescriptionModel()))
                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Role, RoleModel>().ReverseMap();
            CreateMap<Attendance, AttendanceModel>().ReverseMap();
        }
    }
}

