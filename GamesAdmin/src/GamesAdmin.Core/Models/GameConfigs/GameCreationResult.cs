namespace GamesAdmin.Core.Models.GameConfigs
{
    public class GameCreationResult
    {
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public static GameCreationResult CreateSuccess()
        {
            return new GameCreationResult
            {
                Success = true
            };
        }

        public static GameCreationResult CreateError(string errorCode, string errorMessage)
        {
            return new GameCreationResult
            {
                Success = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            };
        }
    }

    public static class GameCreationErrorCodes
    {
        public const string DuplicateGameId = "DUPLICATE_GAME_ID";
        public const string DuplicateGameName = "DUPLICATE_GAME_NAME";
        public const string InvalidGameData = "INVALID_GAME_DATA";
        public const string DatabaseError = "DATABASE_ERROR";
        public const string UnknownError = "UNKNOWN_ERROR";
    }
}
