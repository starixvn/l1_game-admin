using System.Collections.Generic;
using System;

namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class PromotionCampaignViewModel
    {
        public string Id { get; set; }
        public bool Enabled { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now;

        public List<OperatorViewModel> Operators { get; set; }
        public List<WinningPrizeViewModel> WinningPrizes { get; set; }
    }
}
public class OperatorViewModel
{
    public string OperatorId { get; set; }
    public string ExcludedSiteNames { get; set; }
    public List<CurrencyGroupsViewModel> CurrencyGroups { get; set; }
}

public class CurrencyGroupsViewModel
{
    public string CurrencyGroupId { get; set; }
    public double OtherGameMinBet { get; set; }
    public double SlotGameMinBet { get; set; }
    public bool Enabled { get; set; }
    public string CurrencyName { get; set; }
}

public class WinningPrizeViewModel
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Quantity { get; set; }
}