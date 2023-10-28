using System.ComponentModel.DataAnnotations;

namespace MainServer.Application.Models.Login
{
    public class AuthenticateRequest
    {
        [Required]
        public string Mail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
