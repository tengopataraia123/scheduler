using Microsoft.AspNetCore.Mvc;
using ProgramServer.Application.DTOs;
using ProgramServer.Application.Services.Subjects;
using Microsoft.AspNetCore.Authorization;

namespace ProgramServer.Api.Controllers.Subject
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SubjectController : ApiControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpPost("Create")]
        //[Authorize(Roles = "admin,coordinator")]
        public async Task<ActionResult> CreateSubject([FromBody] SubjectCreateModel subjectModel)
        {
            await _subjectService.CreateSubject(subjectModel);
            return Ok();
        }

        [HttpPost("[action]")]
        //[Authorize(Roles = "admin,coordinator")]
        public async Task<IActionResult> AddSubjectUsers([FromBody] List<SubjectUserModel> subjectUsers)
        {
            await _subjectService.AddSubjectUsers(subjectUsers);
            return Ok();
        }

        [HttpGet("GetSubjects")]
        public async Task<IActionResult> GetSubjectsByUser()
        {
            var subjects = await _subjectService.GetSubjectsByUserId(CurrentUser.Id);

            return Ok(subjects);
        }

        [HttpGet("GetAllSubjects")]
        public async Task<ActionResult<List<SubjectCreateModel>>> GetAllSubjects()
        {
            var subjects = await _subjectService.GetAllSubjects();
            return Ok(subjects);
        }

        //[HttpGet("Find/{id}")]
        //public async Task<ActionResult<SubjectModel>> FindSubject([FromRoute] int id)
        //{
        //    var subject = await _subjectService.FindSubject(id);
        //    return Ok(subject);
        //}

        //[HttpDelete("Delete/{id}")]
        //public async Task<ActionResult> DeleteSubject([FromRoute] int id)
        //{
        //    await _subjectService.DeleteSubject(id);
        //    return Ok();
        //}
    }
}

