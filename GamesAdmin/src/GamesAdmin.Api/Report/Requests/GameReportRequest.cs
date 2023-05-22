using GamesAdmin.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace GamesAdmin.Api.Report.Requests
{
    public class GameReportRequest : IRequest<IEnumerable<BaseBetHistory>>
    {
        public long RoundId { get; }

        public int GameType { get; }

        public string Name { get; }

        public GameReportRequest(string name, long roundId, int gameType)
        {
            Name = name;
            RoundId = roundId;
            GameType = gameType;
        }
    }
}