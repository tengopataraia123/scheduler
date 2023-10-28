using System.Text.Json.Serialization;
using ProgramServer.Domain.Common;
using ProgramServer.Domain.Events;
using ProgramServer.Domain.SubjectUsers;

namespace ProgramServer.Domain.Subjects
{
    public class Subject : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string DescriptionStr { get; set; }

        [JsonIgnore]

        public IEnumerable<SubjectUser> SubjectUsers { get; set; }

        public IEnumerable<Event> Events { get; set; }
    }
}

