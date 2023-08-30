using System.Linq;
using AutoMapper;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models;
using System.Threading.Tasks;
using GamesAdmin.Database.Report;
using GamesAdmin.Api.Currencies;
using GamesAdmin.Api.CurrencyGroups;

namespace GamesAdmin.Api.Report
{
    public class ColorGameReportService : IGameReportService, IBetHistoryReportService
    {
        private readonly IColorGameReportRepository repository;
        private readonly IMapper mapper;
        private readonly ICurrencyService currencyService;
        private readonly ICurrencyGroupService currencyGroupService;

        public ColorGameReportService(IColorGameReportRepository repository, IMapper mapper,
            ICurrencyService currencyService,
            ICurrencyGroupService currencyGroupService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.currencyService = currencyService;
            this.currencyGroupService = currencyGroupService;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetRecordAsync(filter);

            return new ColorGameBetReport
            {
                RoundResult = mapper.Map<ColorGameRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<ColorGameBetRecord>(x))
            };
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var result = await repository.GetBetHistoryAsync(filter);

            var betHistory = new ColorGameBetHistory
            {
                RoundResult = mapper.Map<ColorGameRoundResult>(result.Item1),
                Bets = result.Item2.Select(x => mapper.Map<ColorGameBetHistoryRecord>(x)),
                Odds = result.Item3
            };
            if (result.Item2.Any())
            {
                var currency = (await currencyService.GetByPartner(filter.PartnerId)).FirstOrDefault(c => c.Name == result.Item2.FirstOrDefault().Currency);
                var currencyGroup = await currencyGroupService.GetById(currency.GroupId);
                betHistory.FractionalOddsFormat = currencyGroup.FractionalOddsFormat;
            }
            return betHistory;
        }
    }
}