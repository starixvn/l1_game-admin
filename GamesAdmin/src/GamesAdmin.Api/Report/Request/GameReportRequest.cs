using GamesAdmin.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace GamesAdmin.Api.Report.Request
{
    public class GameReportRequest : IRequest<IEnumerable<BaseBetHistory>>
    {
        public long RoundId { get; }

        public byte GameType { get; }

        public string Name { get; }

        public GameReportRequest(string name, long roundId, byte gameType)
        {
            Name = name;
            RoundId = roundId;
            GameType = gameType;
        }
    }
}