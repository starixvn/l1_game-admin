using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Currencies;
using GamesAdmin.Api.CurrencyGroups;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.Report
{
    public class KenoPowerReportService<TKenoPowerRound, TKenoPowerBet, TKenoPowerBetLog> : IGameReportService, IBetHistoryReportService
        where TKenoPowerRound : KenoPowerRoundEntity, new()
        where TKenoPowerBet : KenoPowerOneEntity
        where TKenoPowerBetLog : KenoPowerBetEntity
    {
        private readonly IKenoPowerReportRepository repository;
        private readonly IMapper mapper;
        private readonly IBetHistoryReportRepository betHistoryRepository;
        private readonly ICurrencyService currencyService;
        private readonly ICurrencyGroupService currencyGroupService;

        public KenoPowerReportService(IKenoPowerReportRepository repository,
            IBetHistoryReportRepository betHistoryRepository,
            IMapper mapper,
            ICurrencyService currencyService,
            ICurrencyGroupService currencyGroupService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.betHistoryRepository = betHistoryRepository;
            this.currencyService = currencyService;
            this.currencyGroupService = currencyGroupService;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetAsync<TKenoPowerRound, TKenoPowerBet>(filter);

            return new KenoPowerBetReport
            {
                RoundResult = mapper.Map<KenoPowerRoundResult>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<KenoPowerBetRecord>(x))
            };
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var result = await betHistoryRepository.GetKenoPowerAsync<TKenoPowerRound, TKenoPowerBetLog, TKenoPowerBet>(filter.RoundId, filter.UserId, filter.PartnerId);
            var betHistory = new KenoPowerBetHistory
            {
                RoundResult = mapper.Map<KenoPowerRoundResult>(result.Item1),
                Bets = result.Item2.Select(x => mapper.Map<KenoPowerBetHistoryRecord>(x)),
                Odds = result.Item3
            };
            if (result.Item2.Any())
            {
                var bet = result.Item2.First();
                var currency = (await currencyService.GetByPartner(bet.OperatorId)).FirstOrDefault(c => c.Name == bet.Currency);
                var currencyGroup = await currencyGroupService.GetById(currency.GroupId);
                betHistory.FractionalOddsFormat = currencyGroup.FractionalOddsFormat;
            };

            return betHistory;
        }
    }
}