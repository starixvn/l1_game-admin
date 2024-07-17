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
        public string Status => GetStatusPromotion();
        public List<OperatorModel> Operators { get; set; }
        public List<WinningPrizeModel> WinningPrizes { get; set; }
        public bool HasAnnouncement { get; set; }
        public string GetStatusPromotion()
        {
            DateTime now = DateTime.UtcNow;

            if (StartDateTime <= now && now <= EndDateTime)
            {
                return "Running";
            }
            else if (now > EndDateTime)
            {
                return "Completed";
            }
            else
            {
                return "Not Started";
            }
        }
    } 
}
public class OperatorModel
{
    public string OperatorId { get; set; }
    public string OperatorName { get; set; }
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
    public string Ranking { get; set; }

    public int Value { get; set; }
}
