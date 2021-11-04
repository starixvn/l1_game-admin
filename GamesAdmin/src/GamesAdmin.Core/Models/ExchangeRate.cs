using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class ExchangeRate
    {
        public ExchangeRate(DateTime time, int exId, string source, IDictionary<string, double> code)
        {
            Time = time;
            ExId = exId;
            Source = source;
            Code = code;
        }

        public DateTime Time { get; }

        public int ExId { get; }

        public string Source { get; }

        public IDictionary<string, double> Code { get; }
    }
}