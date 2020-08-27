using System;
using System.Collections.Generic;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Site.Features.Monitors.Models
{
    public class GameRoundInfoCache
    {
        public DateTimeOffset CachedDate { get; set; }

        public Dictionary<string, GameRoundInfo> GameRounds { get; set; }
    }
}
