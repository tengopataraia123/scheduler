using System;
using ProgramServer.Application.DTOs;

namespace ProgramServer.Application.Services.Subjects
{
    public interface ISubjectService
    {
        Task<SubjectModel> FindSubject(int id);
        Task<List<SubjectModel>> GetAllSubjects();
        Task<List<SubjectModel>> GetSubjectsByUserId(int id);
        Task CreateSubject(SubjectCreateModel subject);
        Task DeleteSubject(int id);
        Task AddSubjectUser(SubjectUserModel subjectUser);
    }
}

