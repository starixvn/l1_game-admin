using GamesAdmin.Database.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GamesAdmin.Database.Entities.MiniRoulette
{
    [BsonCollection("miniroulette_betlogs")]
    public class MiniRouletteBetLogEntity : RouletteBetEntity
    {
    }
}
