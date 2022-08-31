using System;
using System.Collections.Generic;
using System.Text;

namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class FruitSlotOdds : Enumeration
    {
        public const string BarValue = "bar";
        public const string Bar50Value = "bar50";
        public const string Bar25Value = "bar25";
        public const string TripleSevenValue = "tripleSeven";
        public const string TripleSeven3Value = "tripleSeven3";
        public const string WatermeloValue = "watermelon";
        public const string Watermelo3Value = "watermelon3";
        public const string StarValue = "star";
        public const string Star3Value = "star3";
        public const string BellValue = "bell";
        public const string Bell3Value = "bell3";
        public const string OrangeValue = "orange";
        public const string Orange3Value = "orange3";
        public const string LemonValue = "lemon";
        public const string Lemon3Value = "lemon3";
        public const string CherryValue = "cherry";
        public const string Cherry3Value = "cherry3";
        public const string BigValue = "big";
        public const string SmallValue = "small";

        public static readonly FruitSlotOdds Bar = new FruitSlotOdds(BarValue, "Bar");
        public static readonly FruitSlotOdds Bar50 = new FruitSlotOdds(Bar50Value, "Bar - x50");
        public static readonly FruitSlotOdds Bar25 = new FruitSlotOdds(Bar25Value, "Bar - x25");
        public static readonly FruitSlotOdds TripleSeven = new FruitSlotOdds(TripleSevenValue, "777");
        public static readonly FruitSlotOdds TripleSeven3 = new FruitSlotOdds(TripleSeven3Value, "777 - x3");
        public static readonly FruitSlotOdds Star = new FruitSlotOdds(StarValue, "Star");
        public static readonly FruitSlotOdds Star3 = new FruitSlotOdds(Star3Value, "Star - x3");
        public static readonly FruitSlotOdds Watermelo = new FruitSlotOdds(WatermeloValue, "Watermelon");
        public static readonly FruitSlotOdds Watermelo3 = new FruitSlotOdds(Watermelo3Value, "Watermelon - x3");
        public static readonly FruitSlotOdds Bell = new FruitSlotOdds(BellValue, "Bell");
        public static readonly FruitSlotOdds Bell3 = new FruitSlotOdds(Bell3Value, "Bell - x3");
        public static readonly FruitSlotOdds Lemon = new FruitSlotOdds(LemonValue, "Lemon");
        public static readonly FruitSlotOdds Lemon3 = new FruitSlotOdds(Lemon3Value, "Lemon - x3");
        public static readonly FruitSlotOdds Orange = new FruitSlotOdds(OrangeValue, "Orange");
        public static readonly FruitSlotOdds Orange3 = new FruitSlotOdds(Orange3Value, "Orange - x3");
        public static readonly FruitSlotOdds Cherry = new FruitSlotOdds(CherryValue, "Cherry");
        public static readonly FruitSlotOdds Cherry3 = new FruitSlotOdds(Cherry3Value, "Cherry - x3");
        public static readonly FruitSlotOdds Big = new FruitSlotOdds(BigValue, "Big");
        public static readonly FruitSlotOdds Small = new FruitSlotOdds(SmallValue, "Small");

        public FruitSlotOdds()
        { }

        public FruitSlotOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}