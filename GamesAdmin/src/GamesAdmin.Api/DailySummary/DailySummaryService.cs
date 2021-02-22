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
        private const int RoundDecimals = 2;

        private readonly IDailySummaryRepository repository;

        public DailySummaryService(IDailySummaryRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<DailySummaryResult>> Get(DateTimeOffset summarizeDate)
        {
            var result = (await repository.FilterByAsync(x => x.Time == summarizeDate.Date)).ToList();

            return result
                .GroupBy(x => x.Currency)
                .Select(g => new DailySummaryResult(
                    g.Key,
                    decimal.Round(g.Sum(i => i.Stake), RoundDecimals),
                    decimal.Round(g.Sum(i => i.Payout), RoundDecimals),
                    g.Sum(i => i.Tickets)));
        }
    }
}