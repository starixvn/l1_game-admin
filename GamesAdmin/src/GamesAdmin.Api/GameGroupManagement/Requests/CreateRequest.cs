using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameGroupManagement.Requests
{
    public class CreateRequest : IRequest<bool>
    {
        public GameGroup GameGroup { get; set; }
    }
}