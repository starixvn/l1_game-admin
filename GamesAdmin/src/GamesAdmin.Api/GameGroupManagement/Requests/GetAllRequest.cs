using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameGroupManagement.Requests
{
    public class GetAllRequest : IRequest<IEnumerable<GameGroup>>
    {
    }
}