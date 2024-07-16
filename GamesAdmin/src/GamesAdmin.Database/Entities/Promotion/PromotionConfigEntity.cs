using System;
using System.Collections.Generic;
using System.Text;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.Promotion
{
    [BsonCollection("promotion_configs")]
    public class PromotionConfigEntity : MiniGameBaseEntity
    {
        public int PromotionId { get; set; }
        public string Name { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Status { get; set; }
        public List<Operator> Operators { get; set; }
        public List<WinningPrize> WinningPrizes { get; set; }
    }
}
public class Operator
{
    public string OperatorId { get; set; }
    public string ExcludedSiteNames { get; set; }
    public List<CurrencyGroup> CurrencyGroups { get; set; }
}

public class CurrencyGroup
{
    public string CurrencyGroupId { get; set; }
    public double OtherGameMinBet { get; set; }
    public double SlotGameMinBet { get; set; }
    public bool Enabled { get; set; }
}

public class WinningPrize
{
    public string Ranking { get; set; }

    public int Value { get; set; }
}
