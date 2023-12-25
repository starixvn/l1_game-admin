namespace GamesAdmin.Core
{
    public static class Constants
    {
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
        public const string TimeFormat = "HH:mm:ss";
    }

    public static class BlackjackGameAction
    {
        public const string Split = "split";

        public const string Stand = "stand";

        public const string Hit = "hit";

        public const string Double = "double";
    }

    public static class CardLabels
    {
        public static readonly string[] DisplayedName = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    }

    public static class SystemType
    {
        public const string SC1 = "SC1";
        public const string VG2 = "VG2";
    }

    public static class BaseCurrencyMultiplier {
        public const int SC1 = 1;
        public const int VG2 = 1000;
    }

    public static class CascadingSlotGameConstant
    {
        public const byte NumberOfRows = 5;
        public const byte NumberOfColumns = 6;
        public const byte MinMultiplierValue = 10;
        public const byte MinMultiplierSymbol = 5;
        public const byte Multiplier10xSymbol = 10;
        public const byte Multiplier15xSymbol = 15;
        public const byte FreeSpinSymbol = 9;
    }

    public static class TreasureIslandGameConstant
    {
        public const byte MinMultiplierSymbol = 10;
        public const byte Multiplier20xSymbol = 20;
        public const byte Multiplier30xSymbol = 30;
    }

    public static class CrashGameAction
    {
        public const string Refund = "refund";
        public const string CashOut = "cashOut";
        public const string HalfCashOut = "halfCashOut";
    }

    public static class Operators
    {
        public const string SabaPlay = "SabaPlay";
    }
}