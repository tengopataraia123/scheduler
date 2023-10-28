using System;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProgramServer.Application.DTOs;
using ProgramServer.Application.Repository;
using ProgramServer.Domain.Surveys;

namespace ProgramServer.Application.Services.Surveys
{
    public class ResponseService : IResponseService
    {
        private readonly IRepository<Response> _responseRepository;
        private readonly ISurveyService _surveyService;
        private readonly IMapper _mapper;

        public ResponseService(IRepository<Response> responseRepository, IMapper mapper, ISurveyService surveyService)
        {
            _responseRepository = responseRepository;
            _surveyService = surveyService;
            _mapper = mapper;
        }
        public async Task CreateResponse(ResponseModel responseModel)
        {
            var response = _mapper.Map<Response>(responseModel);

            _responseRepository.Add(response);
            await _responseRepository.SaveAsync();
        }
        public async Task<List<ResponseModel>> GetResponsesBySubject(string subject)
        {
            var responses = await _responseRepository.Where(x => x.Subject == subject).FirstOrDefaultAsync();

            return _mapper.Map<List<ResponseModel>>(responses);
        }
        public async Task<List<ResponseModel>> GetAllResponses()
        {
            var resposnes = await _responseRepository.GetAll().ToListAsync();
            return _mapper.Map<List<ResponseModel>>(resposnes);
        }
    }
}

