using System;

namespace GamesAdmin.Site.Extensions
{
    public static class NumberFormatExtensions
    {
        private static int Hundredth = 100;
        private static int RoundDigits = 2;

        public static double ToPercentage(this double number)
        => Math.Round(number * Hundredth, RoundDigits);

        public static double Round(this double number)
        => Math.Round(number, RoundDigits);

        public static double ConvertPercentValue(this double number)
        => Math.Round(number / Hundredth, RoundDigits);

        public static double ReversePercent(this double percentage)
        => Hundredth - percentage;
    }
}
