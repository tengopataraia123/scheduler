using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgramServer.Application.DTOs;
using ProgramServer.Application.Services.Events;

namespace ProgramServer.Api.Controllers.Event
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody] EventCreateModel eventModel)
        {
            await _eventService.Add(eventModel);
            return Ok();
        }

        [HttpPost("AddEvents")]
        public async Task<ActionResult> AddEvents([FromBody] List<EventCreateModel> events)
        {
            await _eventService.AddEvents(events);
            return Ok();
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<EventCreateModel>>> GetAll()
        {
            var events = await _eventService.GetAll();
            return Ok(events);
        }

        [HttpDelete("DeleteBySubjectIds")]
        public async Task<ActionResult> DeleteEvents([FromQuery] List<int> subjectIds)
        {
            await _eventService.DeleteEvents(subjectIds);
            return Ok();
        }
    }
}

