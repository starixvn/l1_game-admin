namespace GamesAdmin.Core.Enumeration.CrashGame
{
    public enum GameMode
    {
        Easy = 1,
        Medium,
        Hard,
        HardCore
    }

    public static class GameModeHelper
    {
        public static string GetModeName(byte Mode)
        {
            switch (Mode)
            {
                case (byte)GameMode.Easy:
                    return "Easy";
                case (byte)GameMode.Medium:
                    return "Medium";
                case (byte)GameMode.Hard:
                    return "Hard";
                case (byte)GameMode.HardCore:
                    return "Hard Core";
                default:
                    return "Unknown";
            }
        }
    }

}
