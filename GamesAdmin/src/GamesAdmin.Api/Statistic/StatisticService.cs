using System.Threading.Tasks;
using GamesAdmin.Database;

namespace GamesAdmin.Api.Statistic
{
    public interface IStatisticService
    {
        Task<long> GetTodayTotalBets();
    }

    public class StatisticService : IStatisticService
    {
        private readonly IStatisticRepository statisticRepository;

        public StatisticService(IStatisticRepository statisticRepository)
        {
            this.statisticRepository = statisticRepository;
        }

        public async Task<long> GetTodayTotalBets() => await statisticRepository.GetTodayTotalBets();
    }
}