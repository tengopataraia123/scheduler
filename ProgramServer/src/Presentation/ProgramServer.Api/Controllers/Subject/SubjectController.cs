using System;
using AutoMapper;
using System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgramServer.Application.DTOs;
using ProgramServer.Application.Services.Subjects;

namespace ProgramServer.Api.Controllers.Subject
{
    [ApiController]
    [Route("[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateSubject([FromBody] SubjectCreateModel subjectModel)
        {
            await _subjectService.CreateSubject(subjectModel);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddSubjectUsers([FromBody] List<SubjectUserModel> subjectUsers)
        {
            await _subjectService.AddSubjectUsers(subjectUsers);
            return Ok();
        }

        [HttpGet("GetSubjects/{userId}")]
        public async Task<IActionResult> GetSubjectsByUserId(int userId)
        {
            var subjects = await _subjectService.GetSubjectsByUserId(userId);

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

