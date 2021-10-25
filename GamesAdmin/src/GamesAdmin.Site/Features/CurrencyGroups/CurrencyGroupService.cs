using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.Currencies;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups
{
    public interface ICurrencyGroupService
    {
        Task<IEnumerable<CurrencyGroupModel>> GetAll();
    }

    public class CurrencyGroupService : ICurrencyGroupService
    {
        private readonly IMediator mediator;
        private readonly ICurrencyGroupApi api;

        public CurrencyGroupService(IMediator mediator, ICurrencyGroupApi api)
        {
            this.mediator = mediator;
            this.api = api;
        }

        public Task<IEnumerable<CurrencyGroupModel>> GetAll()
            => api.GetAll();
    }
}