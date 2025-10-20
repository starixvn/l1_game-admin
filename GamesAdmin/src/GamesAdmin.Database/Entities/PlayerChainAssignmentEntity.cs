using System;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("player_chain_assignments")]
    public class PlayerChainAssignmentEntity : MiniGameBaseEntity
    {
        [BsonElement("playerId")]
        public string PlayerId { get; set; }

        [BsonElement("playerName")]
        public string PlayerName { get; set; }

        [BsonElement("operatorId")]
        public string OperatorId { get; set; }

        [BsonElement("gameId")]
        public string GameId { get; set; }

        [BsonElement("chainName")]
        public string ChainName { get; set; }

        [BsonElement("currentBoardIndex")]
        public int CurrentBoardIndex { get; set; }

        [BsonElement("totalBoards")]
        public int TotalBoards { get; set; }

        [BsonElement("balanceBeforeChain")]
        public decimal? BalanceBeforeChain { get; set; }

        [BsonElement("balanceAfterChain")]
        public decimal? BalanceAfterChain { get; set; }

        [BsonElement("currency")]
        public string Currency { get; set; }

        [BsonElement("assignedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime AssignedAt { get; set; }

        [BsonElement("completedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? CompletedAt { get; set; }

        [BsonElement("lastActivityAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime LastActivityAt { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("betLevel")]
        public int BetLevel { get; set; }

        [BsonElement("coinValue")]
        public decimal CoinValue { get; set; }

        [BsonElement("totalWinAmount")]
        public decimal TotalWinAmount { get; set; }

        [BsonElement("totalBetAmount")]
        public decimal TotalBetAmount { get; set; }
    }
}
