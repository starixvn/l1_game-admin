using System.Collections.Generic;
using GamesAdmin.Core.Enumeration.BetChoices;
using MongoDB.Bson;

namespace GamesAdmin.Database.Entities.Keno
{
    public abstract class BaseKenoOnesEntity : BaseBetEntity
    {
        public decimal Big { get; set; }

        public decimal Small { get; set; }

        public decimal Odd { get; set; }

        public decimal Even { get; set; }

        public decimal Gold { get; set; }

        public decimal Earth { get; set; }

        public decimal Fire { get; set; }

        public decimal Water { get; set; }

        public decimal Wood { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }

        public ObjectId UserId { get; set; }

        public Dictionary<string, decimal> ConvertAmountToDictionary()
        => new Dictionary<string, decimal>
            {
                { KenoBetChoice.BigValue,  Big},
                { KenoBetChoice.SmallValue,  Small},
                { KenoBetChoice.OddValue,  Odd},
                { KenoBetChoice.EvenValue,  Even},
                { KenoBetChoice.GoldValue,  Gold},
                { KenoBetChoice.EarthValue,  Earth},
                { KenoBetChoice.FireValue,  Fire},
                { KenoBetChoice.WaterValue,  Water},
                { KenoBetChoice.WoodValue,  Wood},
            };
    }
}
