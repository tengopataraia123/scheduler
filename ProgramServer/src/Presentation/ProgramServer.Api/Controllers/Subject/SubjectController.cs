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

        [HttpPost("CreateSubject")]
        //[Authorize(Roles = "admin,coordinator")]
        public async Task<ActionResult> CreateSubject([FromBody] SubjectCreateModel subjectModel)
        {
            await _subjectService.CreateSubject(subjectModel);
            return Ok();
        }

        [HttpPost("AddSubjectUsers")]
        //[Authorize(Roles = "admin,coordinator")]
        public async Task<IActionResult> AddSubjectUsers([FromBody] List<SubjectUserModel> subjectUsers)
        {
            await _subjectService.AddSubjectUsers(subjectUsers);
            return Ok();
        }



        [HttpGet("GetAllSubjects")]
        public async Task<ActionResult<List<SubjectGetModel>>> GetAllSubjects()
        {
            var subjects = await _subjectService.GetAllSubjects();
            return Ok(subjects);
        }

        [HttpGet("GetAllSubjectUsers/{subjectCode}")]
        public async Task<ActionResult<List<SubjectUserModel>>> GetAllSubjectUsers([FromRoute] string subjectCode)
        {
            var subjects = await _subjectService.GetUsersBySubjectCode(subjectCode);
            return Ok(subjects);
        }


        [HttpDelete("DeleteSubject")]
        public async Task<ActionResult> DeleteSubject([FromRoute] string subjectCode)
        {
            await _subjectService.DeleteSubject(subjectCode);
            return Ok();
        }
    }
}

