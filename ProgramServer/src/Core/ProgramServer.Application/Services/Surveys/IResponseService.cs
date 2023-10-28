using System;
using ProgramServer.Application.DTOs;

namespace ProgramServer.Application.Services.Surveys
{
    public interface IResponseService
    {
        Task<List<ResponseModel>> GetResponsesBySubject(string subject);
        Task CreateResponse(ResponseModel question);
        Task<List<ResponseModel>> GetAllResponses();
    }
}

