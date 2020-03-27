using MediatR;

namespace GamesAdmin.Api.UM.Requests
{
    public class UMRequest : IRequest<bool>
    {
        public UMRequest(bool isUM) 
        {
            IsUM = isUM;
        }

        public bool IsUM { get; }
    }
}
