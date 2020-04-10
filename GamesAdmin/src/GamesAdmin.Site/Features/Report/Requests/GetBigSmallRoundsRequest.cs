using System.Collections.Generic;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetBigSmallRoundsRequest : IRequest<IEnumerable<long>>
    {
    }
}