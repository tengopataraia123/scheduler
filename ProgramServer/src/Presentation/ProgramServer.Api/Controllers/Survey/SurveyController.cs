using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgramServer.Application.DTOs;
using ProgramServer.Application.Services.Surveys;

namespace ProgramServer.Api.Controllers.Survey
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SurveyController : ApiControllerBase
    {
        private readonly ISurveyService _surveyService;
        private readonly IResponseService _responseService;

        public SurveyController(ISurveyService surveyService, IResponseService responseService)
        {
            _surveyService = surveyService;
            _responseService = responseService;
        }
        [HttpPost("CreateResponse")]
        public async Task<ActionResult> CreateResponse([FromBody] ResponseModel responseModel)
        {
            await _responseService.CreateResponse(responseModel);
            return Ok();
        }
        [HttpGet("SurveyResults")]
        public async Task<ActionResult<List<ResponseModel>>> SurveyResults()
        {
            var answers = await _responseService.GetAllResponses();
            return Ok(answers);
        }
        [HttpGet("GetAnswers/{subject}")]
        public async Task<IActionResult> GetAnswersBysubject(string subject)
        {
            var answers = await _responseService.GetResponsesBySubject(subject);

            return Ok(answers);
        }

        [HttpGet("Find/{id}")]
        public async Task<ActionResult<SurveyModel>> FindSurvey([FromRoute] int id)
        {
            var question = await _surveyService.FindQuestion(id);
            return Ok(question);
        }

        [HttpGet("GetAllQuestions")]
        public async Task<ActionResult<List<SurveyModel>>> GetAllSurveys()
        {
            var questions = await _surveyService.GetAllQuestions();
            return Ok(questions);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateSurvey([FromBody] SurveyModel questionModel)
        {
            await _surveyService.CreateQuestion(questionModel);
            return Ok();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteSurvey([FromRoute] int id)
        {
            await _surveyService.DeleteQuestion(id);
            return Ok();
        }
    }
}

