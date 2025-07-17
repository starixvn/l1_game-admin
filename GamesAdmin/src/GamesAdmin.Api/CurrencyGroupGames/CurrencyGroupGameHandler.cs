using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.CurrencyGroupGames.Requests;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroupGames
{
    public class CurrencyGroupGameHandler
        : IRequestHandler<UpdateCurrencyGroupGameRequest, bool>,
        IRequestHandler<GetCurrenctGroupGameRequest, CurrencyGroupGame>,
        IRequestHandler<GetAllRequest, IEnumerable<CurrencyGroupGame>>,
        IRequestHandler<GetByGroupRequest, IEnumerable<CurrencyGroupGame>>,
        IRequestHandler<GetConfigRequest, CurrencyGroupGameConfig>,
        IRequestHandler<UpdateConfigRequest, bool>
    {
        private readonly ICurrencyGroupGameService currencyGroupGameService;

        public CurrencyGroupGameHandler(ICurrencyGroupGameService currencyGroupGameService)
        {
            this.currencyGroupGameService = currencyGroupGameService;
        }

        public Task<bool> Handle(UpdateCurrencyGroupGameRequest request, CancellationToken cancellationToken)
            => this.currencyGroupGameService.Update(request.CurrencyGroupGame);

        public Task<CurrencyGroupGame> Handle(GetCurrenctGroupGameRequest request, CancellationToken cancellationToken)
            => this.currencyGroupGameService.Get(request.GameId, request.CurrencyGroupId);

        public Task<IEnumerable<CurrencyGroupGame>> Handle(GetAllRequest request, CancellationToken cancellationToken)
            => this.currencyGroupGameService.GetAll();

        public Task<IEnumerable<CurrencyGroupGame>> Handle(GetByGroupRequest request, CancellationToken cancellationToken)
            => this.currencyGroupGameService.GetByGroup(request.CurrencyGroupId);

        public Task<CurrencyGroupGameConfig> Handle(GetConfigRequest request, CancellationToken cancellationToken)
            => this.currencyGroupGameService.GetConfig(request.CurrencyGroupId, request.GameId, request.Type);

        public Task<bool> Handle(UpdateConfigRequest request, CancellationToken cancellationToken)
            => this.currencyGroupGameService.UpdateConfig(request.GroupId, request.GameId, request.ConfigValue, request.Type);
    }
}