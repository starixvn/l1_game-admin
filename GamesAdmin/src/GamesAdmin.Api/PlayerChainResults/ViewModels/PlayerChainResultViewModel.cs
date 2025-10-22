using System;
using System.Globalization;
using GamesAdmin.Core.Extensions;

namespace GamesAdmin.Api.PlayerChainResults.ViewModels
{
    public class PlayerChainResultViewModel
    {
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string OperatorId { get; set; }
        public string GameId { get; set; }
        public string ChainName { get; set; }
        public int CurrentBoardIndex { get; set; }
        public int TotalBoards { get; set; }
        public decimal? BalanceBeforeChain { get; set; }
        public decimal? BalanceAfterChain { get; set; }
        public string Currency { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime LastActivityAt { get; set; }
        public string Status { get; set; }
        public int BetLevel { get; set; }
        public decimal CoinValue { get; set; }
        public decimal TotalWinAmount { get; set; }
        public decimal TotalBetAmount { get; set; }
        public string AssignedAtGMT7 => AssignedAt.ToUniversalTime().AddHours(DateTimeExtension.AsianGMT).ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        public string CompletedAtGMT7 => CompletedAt?.ToUniversalTime().AddHours(DateTimeExtension.AsianGMT).ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
    }
}
