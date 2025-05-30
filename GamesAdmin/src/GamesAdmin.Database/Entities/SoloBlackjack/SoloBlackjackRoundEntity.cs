using System;
using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("soloblackjack_rounds")]
    public class SoloBlackjackRoundEntity : BlackjackGameRoundEntity
    {
    }
}