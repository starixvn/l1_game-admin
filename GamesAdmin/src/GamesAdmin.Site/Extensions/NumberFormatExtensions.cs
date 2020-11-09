using System;

namespace GamesAdmin.Site.Extensions
{
    public static class NumberFormatExtensions
    {
        private static int Hundredth = 100;
        private static int RoundDigits = 2;

        public static double ToPercentage(this double number)
        => Math.Round(number * Hundredth, RoundDigits);
    }
}
