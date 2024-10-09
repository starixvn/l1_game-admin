using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameGroupManagement.Requests
{
    public class GetByGroupIdRequest : IRequest<GameGroup>
    {
        public string GroupId { get; set; }
    }
}