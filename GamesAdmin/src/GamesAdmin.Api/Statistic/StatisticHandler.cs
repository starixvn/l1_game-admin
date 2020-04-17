using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Statistic.Requests;
using MediatR;

namespace GamesAdmin.Api.Statistic
{
    public class StatisticHandler : IRequestHandler<GetTodayTotalBetRequest, long>
    {
        private readonly IStatisticService dashboardService;

        public StatisticHandler(IStatisticService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        public async Task<long> Handle(GetTodayTotalBetRequest request, CancellationToken cancellationToken)
        {
            return await dashboardService.GetTodayTotalBets();
        }
    }
}