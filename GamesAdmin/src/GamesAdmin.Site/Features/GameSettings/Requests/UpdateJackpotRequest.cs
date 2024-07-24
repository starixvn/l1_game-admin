using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateJackpotRequest : IRequest<bool>
    {
        public UpdateJackpotRequest(
            string name,
            Jackpot jackpot
           )
        {
            Name = name;
            Jackpot = jackpot;
        }

        public string Name { get; set; }

        public Jackpot Jackpot { get; set; }
    }
}