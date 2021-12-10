using System.Collections.Generic;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateOddsRequest : IRequest<bool>
    {
        public UpdateOddsRequest(string name, Dictionary<string, double> odds)
        {
            Name = name;
            Odds = odds;
        }

        public string Name { get; }

        public Dictionary<string, double> Odds { get; }
    }
}