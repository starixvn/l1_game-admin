using GamesAdmin.Database.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("miniroulette_bets")]
    public class MiniRouletteBetEntity : RouletteOnesEntity
    {
    }
}
