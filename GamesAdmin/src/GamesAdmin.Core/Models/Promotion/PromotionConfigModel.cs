using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.Promotion
{
    public class PromotionConfigModel
    {
        public string Id { get; set; }
        public int PromotionId { get; set; }
        public string Name { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Status { get; set; }
        public List<OperatorModel> Operators { get; set; }
        public List<WinningPrizeModel> WinningPrizes { get; set; }
        public bool HasAnnouncement { get; set; }
    } 
}
public class OperatorModel
{
    public string OperatorId { get; set; }
    public string ExcludedSiteNames { get; set; }
    public List<CurrencyGroupsModel> CurrencyGroups { get; set; }
}

public class CurrencyGroupsModel
{
    public string CurrencyGroupId { get; set; }
    public double OtherGameMinBet { get; set; }
    public double SlotGameMinBet { get; set; }
    public bool Enabled { get; set; }
    public string CurrencyName { get; set; }
}

public class WinningPrizeModel
{
    public string Name { get; set; }

    public int Value { get; set; }

    public int Quantity { get; set; }
}
