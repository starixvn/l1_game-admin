using GamesAdmin.Api.Accounts;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Enumeration.BetChoices;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.KenoProMax;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report.KenoReports
{
    public class KenoProMaxReport : IKenoReport
    {
        private readonly IGenericRepository<KenoProMaxRoundEntity> kenoProMaxRoundRepository;
        private readonly IGenericRepository<KenoProMaxBetEntity> kenoProMaxBetRepository;

        private readonly IAccountService accountService;

        public KenoProMaxReport(
            IGenericRepository<KenoProMaxRoundEntity> kenoProMaxRoundRepository,
            IGenericRepository<KenoProMaxBetEntity> kenoProMaxBetRepository,
            IAccountService accountService)
        {
            this.kenoProMaxRoundRepository = kenoProMaxRoundRepository;
            this.kenoProMaxBetRepository = kenoProMaxBetRepository;

            this.accountService = accountService;
        }

        public async Task<IEnumerable<KenoBetHistory>> GetBetHistoryAsync(long roundId, string nickname)
        {
            var bet = await kenoProMaxBetRepository.FindOneAsync(bet => bet.Round == roundId && bet.Name == nickname);
            var betLogs = new List<KenoBetHistory>();

            foreach (var choice in Enumeration.GetAll<KenoBetChoice>())
            {
                var betAmount = GetBetAmount(choice.Value, bet);

                if (betAmount > 0)
                {
                    betLogs.Add(new KenoBetHistory(
                        bet.Round,
                        bet.Name,
                        choice,
                        GetBetAmount(choice.Value, bet),
                        bet.MemberId,
                        bet.SiteId,
                        bet.Time
                        ));
                }
            }

            return betLogs;
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

        public async Task<IEnumerable<KenoBetRecord>> GetReportAsync(long roundId, string nickname, bool excludeUUS)
        {
            var bets = await kenoProMaxBetRepository.FilterByAsync(x => x.Round == roundId);

            var betRecords = bets
                .Select(bet => new KenoBetRecord(bet.ConvertAmountToDictionary(), bet.Odds)
                {
                    UserId = bet.Uid.ToString(),
                    Nickname = bet.Name,
                    BetWin = bet.Betwin,
                    Time = bet.Time
                })
                .ToList();

            foreach (var bet in betRecords)
            {
                var account = await accountService.GetAccountAsync(bet.UserId, nickname, excludeUUS);
                bet.Username = account?.MemberName;
            }

            return betRecords.Where(bet => bet.Username != null);
        }

        public async Task<KenoRoundResult> GetRoundResultAsync(long roundId)
        {
            var round = await kenoProMaxRoundRepository.FindOneAsync(round => round.Number == roundId);

            return round == null || round.Result == null || round.SettlementResult == null
                ? default
                : new KenoRoundResult(
                    round.Result.ToArray(),
                    new KenoSettlementResult(round.SettlementResult),
                    round.Time);
        }
    }
}
