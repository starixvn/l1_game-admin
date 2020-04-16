using MediatR;

namespace GamesAdmin.Site.Features.Dashboard.Requests
{
    public class GetTodayTotalBetsRequest : IRequest<long>
    {
    }
}