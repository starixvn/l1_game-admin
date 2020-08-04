namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class RouletteBetChoice : Enumeration

    {
        public RouletteBetChoice()
        {
        }

        public RouletteBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }

        public static readonly RouletteBetChoice StraightUp = new RouletteBetChoice("straightUp", "Straight Up");
        public static readonly RouletteBetChoice Split = new RouletteBetChoice("split", "Split");
        public static readonly RouletteBetChoice Street = new RouletteBetChoice("street", "Street");
        public static readonly RouletteBetChoice Triangle = new RouletteBetChoice("triangle", "Triangle");
        public static readonly RouletteBetChoice Corner = new RouletteBetChoice("corner", "Corner");
        public static readonly RouletteBetChoice FourNumbers = new RouletteBetChoice("fourNumbers", "Four Numbers");
        public static readonly RouletteBetChoice Line = new RouletteBetChoice("line", "Line");
        public static readonly RouletteBetChoice Column = new RouletteBetChoice("column", "Column");
        public static readonly RouletteBetChoice Dozen = new RouletteBetChoice("dozen", "Dozen");
        public static readonly RouletteBetChoice Red = new RouletteBetChoice("redBlack", "Red Black");
        public static readonly RouletteBetChoice Odd = new RouletteBetChoice("oddEven", "Odd Even");
        public static readonly RouletteBetChoice Big = new RouletteBetChoice("bigSmall", "Big Small");
    }
}