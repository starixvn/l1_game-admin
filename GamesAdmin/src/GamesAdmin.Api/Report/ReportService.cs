using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;

namespace GamesAdmin.Api.Report
{
    public interface IReportService
    {
        Task<BigSmallReport> GetBigSmallReportAsync(long roundId, string nickname, bool excludeBot);
    }

    public class ReportService : IReportService
    {
        private readonly IBigSmallReportRepository bigSmallReportRepository;

        public ReportService(IBigSmallReportRepository bigSmallReportRepository)
        {
            this.bigSmallReportRepository = bigSmallReportRepository;
        }

        public async Task<BigSmallReport> GetBigSmallReportAsync(long roundId, string nickname, bool excludeBot)
        {
            var result = await bigSmallReportRepository.GetAsync(roundId, nickname, excludeBot);

            return new BigSmallReport
            {
                RoundResult = new RoundResult
                {
                    Number = result.Item1.Number,
                    Dice1 = result.Item1.Dice1,
                    Dice2 = result.Item1.Dice2,
                    Dice3 = result.Item1.Dice3
                },
                Records = result.Item2.Select(x => new BigSmallRecord
                {
                    Time = x.Time,
                    Nickname = x.Nickname,
                    Username = x.Username,
                    SelectBig = x.SelectBig,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Back = x.Back
                })
            };
        }
    }
}