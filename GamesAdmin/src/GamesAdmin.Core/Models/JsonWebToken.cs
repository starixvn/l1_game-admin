namespace GamesAdmin.Core.Models
{
    public class JsonWebToken
    {
        public string Token { get; set; }
    }

    public class RequestJWT
    {
        public RequestJWT(string username, string userAgent)
        {
            Username = username;
            UserAgent = userAgent;
        }

        public string Username { get; set; }

        public string UserAgent { get; set; }
    }
}
