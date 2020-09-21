using System;

namespace GamesAdmin.Core.Models.EndGameInfo
{
    public class EndGameInfoResult
    {
        public string ErrorCode { get; set; } = "0";

        public string ErrorDescription { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TotalWin { get; set; }

        public decimal ValidBetAmount { get; set; }

        public DateTimeOffset? EndTime { get; set; }
    }

    public static class EndGameInfoError
    {
        public readonly static EndGameInfoResult SB1000 = new EndGameInfoResult
        {
            ErrorCode = "SB1000",
            ErrorDescription = "An Error has occurred."
        };

        public readonly static EndGameInfoResult SB1001 = new EndGameInfoResult
        {
            ErrorCode = "SB1001",
            ErrorDescription = "The game round does not exist."
        };

        public readonly static EndGameInfoResult SB1002 = new EndGameInfoResult
        {
            ErrorCode = "SB1002",
            ErrorDescription = "Game round id is required."
        };

        public readonly static EndGameInfoResult SB1003 = new EndGameInfoResult
        {
            ErrorCode = "SB1003",
            ErrorDescription = "Site Id is required."
        };

        public readonly static EndGameInfoResult SB1004 = new EndGameInfoResult
        {
            ErrorCode = "SB1004",
            ErrorDescription = "ObCustId is required."
        };

        public readonly static EndGameInfoResult SB1005 = new EndGameInfoResult
        {
            ErrorCode = "SB1005",
            ErrorDescription = "Game round is running."
        };

        public readonly static EndGameInfoResult SB1006 = new EndGameInfoResult
        {
            ErrorCode = "SB1006",
            ErrorDescription = "GameTypeId is invalid."
        };
    }
}