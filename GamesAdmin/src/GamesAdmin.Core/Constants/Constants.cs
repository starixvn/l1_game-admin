using System.Collections.Generic;

namespace GamesAdmin.Core
{
    public static class Constants
    {
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
        public const string TimeFormat = "HH:mm:ss";
    }

    public static class BlackjackAction
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
}