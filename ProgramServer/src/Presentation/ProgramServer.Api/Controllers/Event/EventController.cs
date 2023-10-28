using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProgramServer.Application.DTOs;
using ProgramServer.Application.Services.Events;

namespace ProgramServer.Api.Controllers.Event
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("Find/{id}")]
        public async Task<ActionResult<EventModel>> FindEvent([FromRoute] int id)
        {
            var eventResult = await _eventService.FindEvent(id);
            return Ok(eventResult);
        }

        [HttpGet("GetAllEvents")]
        public async Task<ActionResult<List<EventModel>>> GetAllEvents()
        {
            var events = await _eventService.GetAllEvents();
            return Ok(events);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateEvent([FromBody] EventCreateModel eventModel)
        {
            await _eventService.CreateEvent(eventModel);
            return Ok();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteEvent([FromRoute] int id)
        {
            await _eventService.DeleteEvent(id);
            return Ok();
        }
    }
}

