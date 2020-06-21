using System.Collections.Generic;
using GamesAdmin.Core.Enumeration.BetChoices;

namespace GamesAdmin.Core.Models.RoundResults
{
    public class KenoRoundResult
    {
        public KenoRoundResult(byte[] results, KenoSettlementResult settlementResult)
        {
            Results = results;
            SettlementResult = settlementResult;
        }

        public byte[] Results { get; }

        public KenoSettlementResult SettlementResult { get; }
    }

    public class KenoSettlementResult
    {
        public KenoSettlementResult(Dictionary<string, bool> settlementResult)
        {
            Big = settlementResult[KenoBetChoice.Big.Value];
            Odd = settlementResult[KenoBetChoice.Odd.Value];
            Gold = settlementResult[KenoBetChoice.Gold.Value];
            Water = settlementResult[KenoBetChoice.Water.Value];
            Wood = settlementResult[KenoBetChoice.Wood.Value];
            Fire = settlementResult[KenoBetChoice.Fire.Value];
            Earth = settlementResult[KenoBetChoice.Earth.Value];
        }

        public KenoSettlementResult(bool big, bool odd, bool gold, bool wood, bool water, bool fire, bool earth)
        {
            Big = big;
            Odd = odd;
            Gold = gold;
            Wood = wood;
            Water = water;
            Fire = fire;
            Earth = earth;
        }

        public bool Big { get; }
        public bool Small => !Big;
        public bool Odd { get; }
        public bool Even => !Odd;
        public bool Gold { get; }
        public bool Wood { get; }
        public bool Water { get; }
        public bool Fire { get; }
        public bool Earth { get; }
    }
}
