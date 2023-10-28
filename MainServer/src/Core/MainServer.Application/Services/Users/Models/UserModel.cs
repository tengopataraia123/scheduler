using MainServer.Application.Services.Programs.Models;

namespace MainServer.Application.Services.Users.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public bool IsBlocked { get; set; }
        public List<ProgramModel> Program { get; set; }
    }
}