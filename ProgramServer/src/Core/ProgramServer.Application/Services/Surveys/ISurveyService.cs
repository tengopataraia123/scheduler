using System;
using ProgramServer.Application.DTOs;

namespace ProgramServer.Application.Services.Surveys
{
    public interface ISurveyService
    {
        Task<SurveyModel> FindQuestion(int id);
        Task<List<SurveyModel>> GetAllQuestions();
        Task CreateQuestion(SurveyModel question);
        Task DeleteQuestion(int id);
    }
}

