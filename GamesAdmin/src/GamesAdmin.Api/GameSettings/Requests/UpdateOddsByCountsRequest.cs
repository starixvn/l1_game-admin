using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateOddsByCountsRequest : IRequest<bool>
    {
        public UpdateOddsByCountsRequest(string name, Dictionary<string, List<OddsByCount>> odds)
        {
            Name = name;
            Odds = odds;
        }

        public string Name { get; }

        public Dictionary<string, List<OddsByCount>> Odds { get; }
    }
}