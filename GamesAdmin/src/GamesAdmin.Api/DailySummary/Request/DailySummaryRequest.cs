using System;
using System.Collections.Generic;
using GamesAdmin.Core.Models.DailySummary;
using MediatR;

namespace GamesAdmin.Api.DailySummary.Request
{
    public class DailySummaryRequest : IRequest<IEnumerable<DailySummaryResult>>
    {
        public DateTimeOffset SummarizedDate { get; set; }
    }
}