using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Enumeration.BetChoices;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.KenoProMax;

namespace GamesAdmin.Api.Report
{
    public class KenoProMaxReportService : IGameReportService, IBetHistoryReportService

    {
        private readonly IMiniKenoReportRepository repository;
        private readonly IGenericRepository<KenoProMaxBetEntity> betLogRepository;

        public KenoProMaxReportService(IMiniKenoReportRepository repository, IGenericRepository<KenoProMaxBetEntity> betLogRepository)
        {
            this.repository = repository;
            this.betLogRepository = betLogRepository;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync<KenoProMaxRoundEntity, KenoProMaxBetEntity>(filter);

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

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var bet = await betLogRepository.FindOneAsync(
                bet => bet.Round == filter.RoundId && bet.Uid == filter.UserId && bet.PartnerId == filter.PartnerId);

            var betLogs = new List<KenoBetHistoryRecord>();

            foreach (var choice in Enumeration.GetAll<KenoBetChoice>())
            {
                var betAmount = GetBetAmount(choice.Value, bet);

                if (betAmount > 0)
                {
                    betLogs.Add(new KenoBetHistoryRecord
                    {
                        Round = bet.Round,
                        Name = bet.Name,
                        Choice = choice,
                        Amount = betAmount,
                        Time = bet.Time,
                        Currency = bet.Currency,
                        PartnerId = bet.PartnerId,
                        Rate = bet.Rate
                    });
                }
            }

            return new KenoBetHistory
            {
                Bets = betLogs
            };
        }

        private decimal GetBetAmount(string betChoice, KenoProMaxBetEntity entity)
        {
            switch (betChoice)
            {
                case KenoBetChoice.BigValue:
                    return entity.Big;

                case KenoBetChoice.SmallValue:
                    return entity.Small;

                case KenoBetChoice.OddValue:
                    return entity.Odd;

                case KenoBetChoice.EvenValue:
                    return entity.Even;

                case KenoBetChoice.GoldValue:
                    return entity.Gold;

                case KenoBetChoice.WoodValue:
                    return entity.Wood;

                case KenoBetChoice.WaterValue:
                    return entity.Water;

                case KenoBetChoice.FireValue:
                    return entity.Fire;

                case KenoBetChoice.EarthValue:
                    return entity.Earth;
            }

            return 0;
        }
    }
}