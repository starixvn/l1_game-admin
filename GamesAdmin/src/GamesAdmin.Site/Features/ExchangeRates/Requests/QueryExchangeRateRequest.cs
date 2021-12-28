using System;
using GamesAdmin.Site.Features.ExchangeRates.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ExchangeRates.Requests
{
    public class QueryExchangeRateRequest : IRequest<ExchangeRateManagementViewModel>
    {
        public QueryExchangeRateRequest(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }

        public DateTime From { get; }

        public DateTime To { get; }
    }
}