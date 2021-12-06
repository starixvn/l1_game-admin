using System;
using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoReportViewModel : ReportResultViewModel
    {
        public KenoReportViewModel(
            GameId game, 
            long roundId, 
            KenoRoundResult roundResult, 
            IEnumerable<KenoRecordViewModel> betTickets,
            TimeZoneInfo timeZoneInfo)
        {
            Game = game;
            RoundId = roundId;
            RoundResult = new KenoResultViewModel(
                roundResult?.Results,
                roundResult?.SettlementResult,
                roundResult?.Time);
            BetTickets = betTickets;
            TimeZoneInfo = timeZoneInfo;
        }

        public GameId Game { get; }

        public long RoundId { get; }

        public KenoResultViewModel RoundResult { get; }

        public IEnumerable<KenoRecordViewModel> BetTickets { get; }

        public TimeZoneInfo TimeZoneInfo { get; }

        public int Total => BetTickets?.Count() == 0 ? 0 : BetTickets.Count();
    }
}
