using System.Collections.Generic;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Database.Report.Entities
{
    public class CascadingSlotBetHistoryEntity
    {
        public CascadingSlotBetHistoryEntity(CascadingSlotRoundEntity round)
        {
            this.Round = new CascadingSlotBetReportEntity 
            {
                BetLevel = round.BetLevel,
                Betwin = round.Betwin,
                CoinValue = round.CoinValue,
                FreeSpin = round.FreeSpin,
                Number = round.Number,
                Multiplier = round.Multiplier,
                Result = round.Result,
                SettlementResult = round.SettlementResult,
                Time = round.Time,
                TotalAmount = round.TotalAmount,
                TotalMultiplier = round.TotalMultiplier,
                TotalTumbleWin = round.TotalTumbleWin,
                ReachMaxWin = round.ReachMaxWin,
                Rate = round.Rate
            };
        }

        public CascadingSlotBetHistoryEntity(CascadingSlotRoundEntity round, IEnumerable<CascadingSlotRoundMultiplierEntity> multipliers)
        {
            this.Round = new CascadingSlotBetReportEntity
            {
                BetLevel = round.BetLevel,
                Betwin = round.Betwin,
                CoinValue = round.CoinValue,
                FreeSpin = round.FreeSpin,
                Number = round.Number,
                Multiplier = round.Multiplier,
                Result = round.Result,
                SettlementResult = round.SettlementResult,
                Time = round.Time,
                TotalAmount = round.TotalAmount,
                TotalMultiplier = round.TotalMultiplier,
                TotalTumbleWin = round.TotalTumbleWin,
                ReachMaxWin = round.ReachMaxWin
            };
            this.Multipliers = multipliers;
        }

        public CascadingSlotBetReportEntity Round { get; set; }

        public IEnumerable<CascadingSlotRoundMultiplierEntity> Multipliers { get; set; } = new List<CascadingSlotRoundMultiplierEntity>();
    }
}
