using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration.BetChoices;
using Newtonsoft.Json;

namespace GamesAdmin.Core.Models.RoundResults
{
    public class KenoRoundResult
    {
        public KenoRoundResult(
            byte[] results,
            KenoSettlementResult settlementResult,
            DateTime time)
        {
            Results = results;
            SettlementResult = settlementResult;
            Time = time;
        }

        public byte[] Results { get; }

        public KenoSettlementResult SettlementResult { get; }

        public DateTime Time { get; }
    }

    public class KenoSettlementResult
    {
        public KenoSettlementResult(int sum)
        { 
        
        }

        public KenoSettlementResult(Dictionary<string, bool> settlementResult)
        {
            Big = settlementResult[KenoBetChoice.Big.Value];
            Small = !Big;
            Odd = settlementResult[KenoBetChoice.Odd.Value];
            Even = !Odd;
            Gold = settlementResult[KenoBetChoice.Gold.Value];
            Water = settlementResult[KenoBetChoice.Water.Value];
            Wood = settlementResult[KenoBetChoice.Wood.Value];
            Fire = settlementResult[KenoBetChoice.Fire.Value];
            Earth = settlementResult[KenoBetChoice.Earth.Value];
        }

        [JsonConstructor]
        public KenoSettlementResult(bool Big, bool Small, bool Odd, bool Even, bool Gold, bool Wood, bool Water, bool Fire, bool Earth)
        {
            this.Big = Big;
            this.Small = Small;
            this.Odd = Odd;
            this.Even = Even;
            this.Gold = Gold;
            this.Wood = Wood;
            this.Water = Water;
            this.Fire = Fire;
            this.Earth = Earth;
        }

        public bool Big { get; }
        public bool Small { get; }
        public bool Odd { get; }
        public bool Even { get; }
        public bool Gold { get; }
        public bool Wood { get; }
        public bool Water { get; }
        public bool Fire { get; }
        public bool Earth { get; }
    }
}
