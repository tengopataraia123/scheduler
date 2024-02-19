using System;
using AutoMapper;
using ProgramServer.Application.Exceptions;
using ProgramServer.Application.Repository;
using System.Linq.Expressions;
using ProgramServer.Domain.Events;
using ProgramServer.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using ProgramServer.Domain.Subjects;
using ProgramServer.Domain.Attandances;
using ProgramServer.Domain.SubjectUsers;
using System.Security.Cryptography;
using System.Text;
using ProgramServer.Domain.Attendances;
using ProgramServer.Domain.Users;

namespace ProgramServer.Application.Services.Events
{
    public class EventService: IEventService
    { 
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<Subject> _subjectRepository;
        private readonly IMapper _mapper;

        public EventService(IRepository<Event> eventRepository,
            IRepository<Subject> subjectRepository,
            IMapper mapper)
        {
            _eventRepository = eventRepository;
            _subjectRepository = subjectRepository;
            _mapper = mapper;
        }

        public async Task Add(EventCreateModel eventModel)
        {
            var validator = new EventCreateModelValidator();
            var result = validator.Validate(eventModel);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
            
            var subject = await _subjectRepository.Where(o => o.Code == eventModel.SubjectCode).FirstOrDefaultAsync();
            eventModel.SubjectId = subject.Id;
            //var eventEntity = _mapper.Map<Event>(eventModel);
            var eventEntity = new Event
            {
                SubjectId = eventModel.SubjectId,
                StartDate = EnsureUtc(eventModel.StartDate),
                EndDate = EnsureUtc(eventModel.EndDate),
            };
            var subjectUsers = await _subjectRepository.Where(o => o.Id == subject.Id)
                .Include(o => o.SubjectUsers)
                .ThenInclude(o => o.User).FirstOrDefaultAsync();

            var users = subjectUsers.SubjectUsers.Select(o => o.User).ToList();

            _eventRepository.Add(eventEntity);
            await _eventRepository.SaveAsync();
        }

        public async Task AddEvents(List<EventCreateModel> events)
        {
            var validator = new EventCreateModelValidator();
            foreach (var eventModel in events)
            {
                var result = validator.Validate(eventModel);

                if (!result.IsValid)
                    throw new ValidationException(result.Errors);

                //var eventEntity = _mapper.Map<Event>(eventModel);

                await Add(eventModel);

                //await _eventRepository.SaveAsync();
            }
        }

        public async Task<List<EventCreateModel>> GetAll()
        {
            var allevents = await _eventRepository.GetAll().ToListAsync();
            return _mapper.Map<List<EventCreateModel>>(allevents);
        }

        private DateTime EnsureUtc(DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }
        public async Task DeleteEvent(List<int> subjectIds)
        {
            await _eventRepository.Delete(o=> subjectIds.Contains(o.SubjectId));
        }

        //public async Task<EventModel> FindEvent(int id)
        //{
        //    var eventResult = await _eventRepository.Where(o => o.Id == id).FirstOrDefaultAsync();
        //    if (eventResult == null)
        //        throw new NotFoundException(nameof(Event), id);

        //    return _mapper.Map<EventModel>(eventResult);
        //}
    }
}

