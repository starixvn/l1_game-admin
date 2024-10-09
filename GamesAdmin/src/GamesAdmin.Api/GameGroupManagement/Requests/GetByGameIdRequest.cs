using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameGroupManagement.Requests
{
    public class GetByGameIdRequest : IRequest<GameGroup>
    {
        public int GameId { get; set; }
    }
}