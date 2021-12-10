using System;

namespace GamesAdmin.Site.Features.ExchangeRates.Models
{
    public class QueryExchangeRateModel
    {
        public QueryExchangeRateModel(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }

        public DateTime From { get; }

        public DateTime To { get; }
    }
}