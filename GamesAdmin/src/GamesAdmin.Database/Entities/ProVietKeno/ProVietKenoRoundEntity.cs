using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("kenoproviet_phiens")]
    public class ProVietKenoRoundEntity : BaseRoundEntity
    {
        public IEnumerable<byte> Result { get; set; }

        public SettlementResult SettlementResult { get; set; }

        public bool Settled { get; set; }
    }

    public class SettlementResult
    {
        public SettlementResult()
        {
        }

        public SettlementResult(int sum)
        {
            SetElementValue(sum);
            SetBigSmallValue(sum);
            SetOddEvenValue(sum);
        }

        public bool Big { get; set; }
        public bool Small { get; set; }
        public bool Odd { get; set; }
        public bool Even { get; set; }
        public bool Gold { get; set; }
        public bool Wood { get; set; }
        public bool Water { get; set; }
        public bool Fire { get; set; }
        public bool Earth { get; set; }

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
            if (sum % 2 == 0)
            {
                Even = true;
            }
            else
            {
                Odd = true;
            }
        }

        private void SetBigSmallValue(int sum)
        {
            if (sum >= 811)
            {
                Big = true;
            }
            else
            {
                Small = true;
            }
        }
    }
}