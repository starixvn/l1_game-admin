namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class VipSeDieWarBetChoice : Enumeration
    {
        public const string PlayerValue = "player";
        public const string BankerValue = "banker";
        public const string TieValue = "tie";
        public const string EvenValue = "even";
        public const string OddValue = "odd";

        public const string PlayerEvenValue = "playerEven";
        public const string PlayerOddValue = "playerOdd";
        public const string PlayerRed3Value = "playerRed3";
        public const string PlayerWhite3Value = "playerWhite3";
        public const string PlayerRed4Value = "playerRed4";
        public const string PlayerWhite4Value = "playerWhite4";

        public const string BankerEvenValue = "bankerEven";
        public const string BankerOddValue = "bankerOdd";
        public const string BankerRed3Value = "bankerRed3";
        public const string BankerWhite3Value = "bankerWhite3";
        public const string BankerRed4Value = "bankerRed4";
        public const string BankerWhite4Value = "bankerWhite4";

        public static readonly VipSeDieWarBetChoice Player = new VipSeDieWarBetChoice(PlayerValue, "Player");
        public static readonly VipSeDieWarBetChoice Banker = new VipSeDieWarBetChoice(BankerValue, "Banker");
        public static readonly VipSeDieWarBetChoice Tie = new VipSeDieWarBetChoice(TieValue, "Tie");
        public static readonly VipSeDieWarBetChoice Even = new VipSeDieWarBetChoice(EvenValue, "Even");
        public static readonly VipSeDieWarBetChoice Odd = new VipSeDieWarBetChoice(OddValue, "Odd");

        public static readonly VipSeDieWarBetChoice PlayerEven = new VipSeDieWarBetChoice(PlayerEvenValue, "Player Even");
        public static readonly VipSeDieWarBetChoice PlayerOdd = new VipSeDieWarBetChoice(PlayerOddValue, "Player Odd");
        public static readonly VipSeDieWarBetChoice PlayerRed3 = new VipSeDieWarBetChoice(PlayerRed3Value, "Player Red3");
        public static readonly VipSeDieWarBetChoice PlayerWhite3 = new VipSeDieWarBetChoice(PlayerWhite3Value, "Player White3");
        public static readonly VipSeDieWarBetChoice PlayerRed4 = new VipSeDieWarBetChoice(PlayerRed4Value, "Player Red4");
        public static readonly VipSeDieWarBetChoice PlayerWhite4 = new VipSeDieWarBetChoice(PlayerWhite4Value, "Player White4");

        public static readonly VipSeDieWarBetChoice BankerEven = new VipSeDieWarBetChoice(BankerEvenValue, "Banker Even");
        public static readonly VipSeDieWarBetChoice BankerOdd = new VipSeDieWarBetChoice(BankerOddValue, "Banker Odd");
        public static readonly VipSeDieWarBetChoice BankerRed3 = new VipSeDieWarBetChoice(BankerRed3Value, "Banker Red3");
        public static readonly VipSeDieWarBetChoice BankerWhite3 = new VipSeDieWarBetChoice(BankerWhite3Value, "Banker White3");
        public static readonly VipSeDieWarBetChoice BankerRed4 = new VipSeDieWarBetChoice(BankerRed4Value, "Banker Red4");
        public static readonly VipSeDieWarBetChoice BankerWhite4 = new VipSeDieWarBetChoice(BankerWhite4Value, "Banker White4");


        public VipSeDieWarBetChoice() { }

        public VipSeDieWarBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
