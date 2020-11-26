namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class VipFishPrawnCrabBetChoice : Enumeration
    {
        public const string FishValue = "fish";
        public const string PrawnValue = "prawn";
        public const string CrabValue = "crab";
        public const string StagValue = "stag";
        public const string GourdValue = "gourd";
        public const string RoosterValue = "rooster";
        public const string AnyTripleValue = "anyTriple";
        public const string MountainValue = "mountain";
        public const string OceanValue = "ocean";

        public static readonly VipFishPrawnCrabBetChoice Fish = new VipFishPrawnCrabBetChoice(FishValue, "Fish");
        public static readonly VipFishPrawnCrabBetChoice Prawn = new VipFishPrawnCrabBetChoice(PrawnValue, "Prawn");
        public static readonly VipFishPrawnCrabBetChoice Crab = new VipFishPrawnCrabBetChoice(CrabValue, "Crab");
        public static readonly VipFishPrawnCrabBetChoice Stag = new VipFishPrawnCrabBetChoice(StagValue, "Stag");
        public static readonly VipFishPrawnCrabBetChoice Gourd = new VipFishPrawnCrabBetChoice(GourdValue, "Gourd");
        public static readonly VipFishPrawnCrabBetChoice Rooster = new VipFishPrawnCrabBetChoice(RoosterValue, "Rooster");
        public static readonly VipFishPrawnCrabBetChoice AnyTriple = new VipFishPrawnCrabBetChoice(AnyTripleValue, "Any Triple");
        public static readonly VipFishPrawnCrabBetChoice Mountain = new VipFishPrawnCrabBetChoice(MountainValue, "Mountain");
        public static readonly VipFishPrawnCrabBetChoice Ocean = new VipFishPrawnCrabBetChoice(OceanValue, "Ocean");

        public VipFishPrawnCrabBetChoice() { }

        public VipFishPrawnCrabBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
