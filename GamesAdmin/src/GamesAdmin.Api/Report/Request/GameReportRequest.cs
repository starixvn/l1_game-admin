using GamesAdmin.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace GamesAdmin.Api.Report.Request
{
    public class GameReportRequest : IRequest<IList<Bet>>
    {
        public long RoundId { get; }

        public byte GameType { get; }

        public string Name { get; }

        public DateTime From { get; }

        public DateTime To { get; }

        public GameReportRequest(string name, long roundId, byte gameType, DateTime from, DateTime to)
        {
            Name = name;
            RoundId = roundId;
            GameType = gameType;
            From = from;
            To = to;
        }
    }
}