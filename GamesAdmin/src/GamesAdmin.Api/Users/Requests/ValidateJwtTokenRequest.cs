using MediatR;

namespace GamesAdmin.Api.Users.Requests
{
    public class ValidateJwtTokenRequest : IRequest<bool>
    {
        public ValidateJwtTokenRequest(string token)
        {
            Token = token;
        }

        public string Token { get; }
    }
}