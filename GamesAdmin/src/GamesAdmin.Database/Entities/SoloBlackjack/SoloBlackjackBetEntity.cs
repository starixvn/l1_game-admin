using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("soloblackjack_bets")]
    public class SoloBlackjackBetEntity : BlackjackGameBetEntity
    {
    }
}