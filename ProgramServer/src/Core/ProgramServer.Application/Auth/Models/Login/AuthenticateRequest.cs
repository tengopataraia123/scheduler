using System.ComponentModel.DataAnnotations;

namespace ProgramServer.Application.Models.Login
{
    public class AuthenticateRequest
    {
        [Required]
        public string Mail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
