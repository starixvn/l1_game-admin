using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class ExchangeRate
    {
        public ExchangeRate(DateTime time, DateTime exDate, int exId, string source, IDictionary<string, decimal> code)
        {
            Time = time;
            ExId = exId;
            Source = source;
            Code = code;
            ExDate = exDate;
        }

        public DateTime Time { get; }

        public DateTime ExDate { get; set; }

        public int ExId { get; }

        public string Source { get; }

        public IDictionary<string, decimal> Code { get; }
    }
}