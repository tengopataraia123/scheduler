using System;
using ProgramServer.Application.DTOs;

namespace ProgramServer.Application.Services.Events
{
    public interface IEventService
    {
        Task<int> Add(EventCreateModel eventModel);
        Task AddEvents(List<EventCreateModel> events);
        Task<List<EventGetModel>> GetAll();
        Task DeleteEvents(List<int> eventIds);
    }
}

