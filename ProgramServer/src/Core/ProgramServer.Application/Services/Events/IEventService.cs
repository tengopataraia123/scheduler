using System;
using ProgramServer.Application.DTOs;

namespace ProgramServer.Application.Services.Events
{
    public interface IEventService
    {
        Task Add(EventCreateModel eventModel);
        Task AddEvents(List<EventCreateModel> events);
        Task<List<EventCreateModel>> GetAll();
        
        //Task<EventModel> FindEvent(int id);
        Task DeleteEvents(List<int> subjectIds);
    }
}

