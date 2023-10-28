using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MainServer.Application.Services.Programs.Contracts;
using MainServer.Application.Services.Programs.Models;
using MainServer.Application.Exceptions;
using System.Security.Claims;

namespace MainServer.Api.Controllers.Program
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProgramController : ApiControllerBase
    { 
        private readonly IProgramService _programService;

        public ProgramController(IProgramService programService)
        {
            _programService = programService;
        }

        [HttpGet("Find/{id}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<ProgramModel>> FindProgram([FromRoute] int id)
        {
            var program = await _programService.Find(id);
            return Ok(program);
        }

        [AllowAnonymous]
        [HttpGet("GetAllPrograms")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<ProgramModel>>> GetAllPrograms()
        {
            var programs = await _programService.GetAll();
            return Ok(programs);
        }

        [HttpPost("Create")]
        [Authorize(Roles = "User, Admin")]
        public async Task<ActionResult> CreateProgram([FromBody] ProgramCreateModel programModel)
        {
            var userId = Convert.ToInt32(User.Claims.Where(o=>o.Type == ClaimTypes.NameIdentifier).First().Value);
            await _programService.Create(programModel,userId);
            return Ok();
        }

        [HttpPut("Activate/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ActivateProgram([FromRoute] int id)
        {
            //var userId = Convert.ToInt32(User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).First().Value);
            await _programService.Activate(id, UserInfo);
            return Ok();
        }

        [HttpPut("Deactivate/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeactivateProgram([FromRoute] int id)
        {
            await _programService.Deactivate(id, UserInfo);
            return Ok();
        }
        
        [AllowAnonymous]
        [HttpGet("[action]/{programId}")]
        public async Task<IActionResult> GenerateNewKey(int programId)
        {
            try
            {
                return Ok(await _programService.GenerateNewKeyPair(programId));
            }catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("[action]/{programId}")]
        public async Task<IActionResult> GetPrivateKey(int programId)
        {
            try
            {
                return Ok(await _programService.GetPrivateKey(programId));
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("[action]/{programId}")]
        public async Task<IActionResult> GetPublicKey(int programId)
        {
            try
            {
                return Ok(await _programService.GetPublicKey(programId));
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("Url/{code}")]
        public async Task<ActionResult<ProgramModel>> FindProgram([FromRoute] string code)
        {
            var url = await _programService.GetProgramUrl(code);
            return Ok(url);
        }

        [HttpPut("Block/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> BlockProgram([FromRoute] int id)
        {
            await _programService.Block(id, UserInfo);
            return Ok();
        }

        [HttpPut("Unblock/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UnblockProgram([FromRoute] int id)
        {
            await _programService.Unblock(id, UserInfo);
            return Ok();
        }
    }
}