using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.DailySummary;
using GamesAdmin.Database;

namespace GamesAdmin.Api.DailySummarize
{
    public interface IDailySummaryService
    {
        Task<IEnumerable<DailySummaryResult>> Get(DateTimeOffset summarizeDate);
    }

    public class DailySummaryService : IDailySummaryService
    {
        private readonly IDailySummaryRepository repository;

        public DailySummaryService(IDailySummaryRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<DailySummaryResult>> Get(DateTimeOffset summarizeDate)
        {
            var fromTime = summarizeDate.DateTime;
            var toTime = fromTime.AddDays(1);
            var result = (await repository.FilterByAsync(x => x.Time >= fromTime && x.Time < toTime)).ToList();

            return result
                .GroupBy(x => x.Currency)
                .Select(g => new DailySummaryResult(
                    g.Key,
                    decimal.Round(g.Sum(i => i.Stake), 2),
                    decimal.Round(g.Sum(i => i.Payout), 2),
                    g.Sum(i => i.Tickets)));
        }
    }
}