using System;
using ProgramServer.Application.DTOs;

namespace ProgramServer.Application.Services.Events
{
    public interface IEventService
    {
        Task<EventModel> FindEvent(int id);
        Task<List<EventModel>> GetAllEvents();
        Task CreateEvent(EventCreateModel user);
        Task DeleteEvent(int id);
    }
}

