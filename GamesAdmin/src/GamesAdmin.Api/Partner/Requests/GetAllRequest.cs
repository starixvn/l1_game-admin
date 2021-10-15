using System.Collections.Generic;
using GamesAdmin.Core.Models.Partner;
using MediatR;

namespace GamesAdmin.Api.Partner.Requests
{
    public class GetAllRequest : IRequest<IEnumerable<PartnerInfo>>
    {
    }
}
