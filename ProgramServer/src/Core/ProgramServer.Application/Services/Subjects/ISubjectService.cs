using System;
using ProgramServer.Application.DTOs;

namespace ProgramServer.Application.Services.Subjects
{
    public interface ISubjectService
    {
        Task CreateSubject(SubjectCreateModel subject);
        Task AddSubjectUsers(List<SubjectUserModel> subjectUser);
        Task<List<SubjectCreateModel>> GetSubjectsByUserId(int id);
        Task<List<SubjectCreateModel>> GetAllSubjects();
        //Task<SubjectModel> FindSubject(int id);
        //Task DeleteSubject(int id);
    }
}

