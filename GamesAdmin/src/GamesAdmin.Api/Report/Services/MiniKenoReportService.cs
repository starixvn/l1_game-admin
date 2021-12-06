using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Keno;

namespace GamesAdmin.Api.Report
{
    public class MiniKenoReportService<TMiniKenoRound, TMiniKenoBet> : IGameReportService
        where TMiniKenoRound : BaseKenoRoundEntity, new()
        where TMiniKenoBet : BaseKenoOnesEntity
    {
        private readonly IMiniKenoReportRepository repository;

        public MiniKenoReportService(IMiniKenoReportRepository repository)
        {
            this.repository = repository;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync<TMiniKenoRound, TMiniKenoBet>(filter);

            return new MiniKenoBetReport
            {
                RoundResult = new KenoRoundResult(
                    result.Item1.Result.ToArray(),
                    new KenoSettlementResult(result.Item1.SettlementResult),
                     result.Item1.Time)
                {
                    Number = result.Item1.Number
                },
                Records = result.Item2.Select(bet => new KenoBetRecord(bet.ConvertAmountToDictionary(), bet.Odds)
                {
                    UserId = bet.UserId,
                    Nickname = bet.Nickname,
                    Username = bet.Username,
                    BetWin = bet.BetWin,
                    Time = bet.Time,
                    Currency = bet.Currency,
                    PartnerId = bet.PartnerId,
                    Rate = bet.Rate
                })
            };
        }
    }
}