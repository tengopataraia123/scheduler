namespace MainServer.Application.Models.Login
{
    public class AuthenticateResponse
    {

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(SystemUserModel user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Token = user.Token;
        }
    }
}

