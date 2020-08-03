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

        public static readonly RouletteBetChoice StraightUp = new RouletteBetChoice("straightup", "Straight Up");
        public static readonly RouletteBetChoice Split = new RouletteBetChoice("split", "Split");
        public static readonly RouletteBetChoice Street = new RouletteBetChoice("street", "Street");
        public static readonly RouletteBetChoice Triangle = new RouletteBetChoice("triangle", "Triangle");
        public static readonly RouletteBetChoice Corner = new RouletteBetChoice("corner", "Corner");
        public static readonly RouletteBetChoice FourNumbers = new RouletteBetChoice("fournumbers", "Four Numbers");
        public static readonly RouletteBetChoice Line = new RouletteBetChoice("line", "Line");
        public static readonly RouletteBetChoice Column = new RouletteBetChoice("column", "Column");
        public static readonly RouletteBetChoice Dozen = new RouletteBetChoice("dozen", "Dozen");
        public static readonly RouletteBetChoice Red = new RouletteBetChoice("red", "Red");
        public static readonly RouletteBetChoice Black = new RouletteBetChoice("black", "Black");
        public static readonly RouletteBetChoice Odd = new RouletteBetChoice("odd", "Odd");
        public static readonly RouletteBetChoice Even = new RouletteBetChoice("even", "Even");
        public static readonly RouletteBetChoice Big = new RouletteBetChoice("big", "Big");
        public static readonly RouletteBetChoice Small = new RouletteBetChoice("small", "Small");

    }
}