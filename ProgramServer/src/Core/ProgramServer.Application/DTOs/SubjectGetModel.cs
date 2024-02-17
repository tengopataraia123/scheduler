using System;
using FluentValidation;

namespace ProgramServer.Application.DTOs
{
    public class SubjectGetModel
    {
        public int SubjectId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DescriptionModel Description { get; set; }
    }
}

