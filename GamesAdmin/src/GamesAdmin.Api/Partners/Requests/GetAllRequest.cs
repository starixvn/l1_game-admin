using System.Collections.Generic;
using GamesAdmin.Core.Models.Partners;
using MediatR;

namespace GamesAdmin.Api.Partners.Requests
{
    public class GetAllRequest : IRequest<IEnumerable<Partner>>
    {
    }
}
