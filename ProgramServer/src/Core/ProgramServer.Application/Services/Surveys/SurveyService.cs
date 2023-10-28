using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProgramServer.Application.DTOs;
using ProgramServer.Application.Repository;
using ProgramServer.Domain.Surveys;

namespace ProgramServer.Application.Services.Surveys
{
    public class SurveyService : ISurveyService
	{
        private readonly IRepository<Survey> _surveyRepository;
        private readonly IMapper _mapper;

        public SurveyService(IRepository<Survey> surveyRepository, IMapper mapper)
        {
            _surveyRepository = surveyRepository;
            _mapper = mapper;
        }

        public async Task CreateQuestion(SurveyModel questionModel)
        {
            var question = _mapper.Map<Survey>(questionModel);
            _surveyRepository.Add(question);
            await _surveyRepository.SaveAsync();
        }

        public async Task DeleteQuestion(int id)
        {
            await _surveyRepository.Delete(o=>o.Id == id);
        }

        public async Task<SurveyModel> FindQuestion(int id)
        {
            var question = await _surveyRepository.Where(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<SurveyModel>(question);
        }

        public async Task<List<SurveyModel>> GetAllQuestions()
        {
            var questions = await _surveyRepository.GetAll().ToListAsync();
            return _mapper.Map<List<SurveyModel>>(questions);
        }
    }
}

