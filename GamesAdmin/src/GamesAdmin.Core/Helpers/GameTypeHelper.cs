using GamesAdmin.Core.Enumeration;
using System.Collections.Generic;

namespace GamesAdmin.Core.Helpers
{
    public static class GameTypeHelper
    {
        private static readonly HashSet<GameType> AccelerateGameTypes = new HashSet<GameType>
        {
            GameType.Slots,
            GameType.CascadingSlots,
            GameType.Crash,
            GameType.CrashTurnBase,
            GameType.CascadingClusterSlots,
            GameType.FixedStepsCrashTurnBase
        };

        public static bool IsAccelerateGameType(GameType gameType)
        {
            return AccelerateGameTypes.Contains(gameType);
        }

        public static bool IsCrashGame(GameType gameType)
        {
            return gameType == GameType.Crash ||
                   gameType == GameType.CrashTurnBase ||
                   gameType == GameType.FixedStepsCrashTurnBase;
        }

        public static bool IsCrashGame(byte gameType)
        {
            return gameType == (byte)GameType.Crash ||
                   gameType == (byte)GameType.CrashTurnBase ||
                   gameType == (byte)GameType.FixedStepsCrashTurnBase;
        }

        public static bool IsCrashTurn(GameType gameType)
        {
            return gameType == GameType.CrashTurnBase ||
                   gameType == GameType.FixedStepsCrashTurnBase;
        }
    }
}
