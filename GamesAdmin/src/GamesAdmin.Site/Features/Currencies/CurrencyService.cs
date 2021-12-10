using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features.Currencies.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies
{
    public interface ICurrencyService
    {
        Task<IEnumerable<SelectListItem>> GetCurrenciesForSelection(Func<CurrencyModel, bool> criteria);

        Task<IEnumerable<CurrencyModel>> GetAll();
    }

    public class CurrencyService : ICurrencyService
    {
        private readonly IMediator mediator;
        private readonly ICurrencyApi api;

        public CurrencyService(IMediator mediator, ICurrencyApi api)
        {
            this.mediator = mediator;
            this.api = api;
        }

        public Task<IEnumerable<CurrencyModel>> GetAll()
            => api.GetAll();

        public Task<IEnumerable<SelectListItem>> GetCurrenciesForSelection(Func<CurrencyModel, bool> criteria)
            => mediator.Send(new GetCurrenciesForSelectionRequest(criteria));
    }
}