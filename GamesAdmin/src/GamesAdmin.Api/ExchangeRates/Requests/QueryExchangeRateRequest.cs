using System;
using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.ExchangeRates.Requests
{
    public class QueryExchangeRateModel
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }

    public class QueryExchangeRateRequest : IRequest<IEnumerable<ExchangeRate>>
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