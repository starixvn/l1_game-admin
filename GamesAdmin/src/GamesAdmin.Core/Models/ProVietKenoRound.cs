using System;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Core.Models
{
    public class ProVietKenoRound
    {
        public ProVietKenoRound(DateTime time, long number, IEnumerable<byte> results)
        {
            Time = time;
            Number = number;
            Results = results;

            var sum = Results == null ? 0 : Results.Sum(x => x);
            SetElementValue(sum);
            SetOddEvenValue(sum);
            SetBigSmallValue(sum);
        }

        public DateTime Time { get; }

        public long Number { get; }

        public IEnumerable<byte> Results { get; }

        public bool Big { get; private set; }
        public bool Small { get; private set; }
        public bool Odd { get; private set; }
        public bool Even { get; private set; }
        public bool Gold { get; private set; }
        public bool Wood { get; private set; }
        public bool Water { get; private set; }
        public bool Fire { get; private set; }
        public bool Earth { get; private set; }


        private void SetElementValue(int sum)
        {
            if (sum >= 210 && sum <= 695)
            {
                Gold = true;
            }

            if (sum >= 696 && sum <= 763)
            {
                Wood = true;
            }

            if (sum >= 764 && sum <= 855)
            {
                Water = true;
            }

            if (sum >= 856 && sum <= 923)
            {
                Fire = true;
            }

            if (sum >= 924 && sum <= 1410)
            {
                Earth = true;
            }
        }

        private void SetOddEvenValue(int sum)
        {
            Even = sum % 2 == 0;
            Odd = !Even;
        }

        private void SetBigSmallValue(int sum)
        {
            Big = sum >= 811;
            Small = !Big;
        }
    }
}