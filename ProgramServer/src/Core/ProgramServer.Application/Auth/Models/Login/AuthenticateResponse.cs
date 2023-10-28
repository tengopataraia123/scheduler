namespace ProgramServer.Application.Models.Login
{
    public class AuthenticateResponse
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(SystemUserModel user)
        {
            Id = user.Id;
            Name = user.Name;
            Token = user.Token;
        }
    }
}

