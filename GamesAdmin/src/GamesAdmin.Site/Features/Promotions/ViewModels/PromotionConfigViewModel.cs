using System.Collections.Generic;
using System.Globalization;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class PromotionConfigViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int PromotionId { get; set; }
        public List<WinningPrizeViewModel> WinningPrizes { get; set; }
        public List<OperatorViewModel> Operators { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Status { set; get; }
        public bool HasAnnouncement { get; set; }
        public string StartDateTimeString => StartDateTime.ToString("MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
        public string EndDateTimeString => EndDateTime.ToString("MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);

        public string GetPrizesHtml()
        {
            var html = string.Join("", WinningPrizes.Select(p => $"<div>Ranking {p.Ranking}: {p.DisplayValue}</div>"));
            return html;
        }

        public string GetTournamentHtml(bool hasAdminPermission, IUrlHelper urlHelper)
        {
            var html = "";
            var rowspanDefault = 1;
            bool isFirstPromotionRow = true;
            var promotionRowSpan = Operators.Sum(o => o.CurrencyGroups.Count(cg => cg.Enabled) > 0 ? o.CurrencyGroups.Count(cg => cg.Enabled) : 1);
            foreach (var operatorModel in Operators)
            {
                var enabledCurrencyGroups = operatorModel.CurrencyGroups.Where(cg => cg.Enabled).ToList();
                var operatorRowSpan = enabledCurrencyGroups.Count > 0 ? enabledCurrencyGroups.Count : 1;
                bool isFirstOperatorRow = true;

                if (enabledCurrencyGroups.Count() > 0)
                {
                    foreach (var currencyGroup in enabledCurrencyGroups)
                    {
                        html += "<tr>";
                        if (isFirstPromotionRow)
                        {
                            html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center\">{Name}</td>";
                            html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center\">{PromotionId}</td>";
                            html += $"<td rowspan=\"{promotionRowSpan}\" width=\"150px\">{GetPrizesHtml()}</td>";
                        }

                        if (isFirstOperatorRow)
                        {
                            html += $"<td rowspan=\"{operatorRowSpan}\">{operatorModel.OperatorName}</td>";
                        }

                        html += $"<td width=\"200px\">" +
                                $"<div><strong>{currencyGroup.CurrencyName}</strong></div>" +
                                $"<div>Slot games: {currencyGroup.DisplaySlotGameMinBet}</div>" +
                                $"<div>Other games: {currencyGroup.DisplayOtherGameMinBet}</div>" +
                                "</td>";                      

                        if (isFirstOperatorRow)
                        {
                            if (hasAdminPermission)
                            {
                                html += $"<td class=\"text-center\" rowspan=\"{operatorRowSpan}\">" +
                                    $"<i class=\"icon-edit\" onclick='promotion.showOperatorSetting(\"{Id}\", \"{operatorModel.OperatorId}\", \"{operatorModel.OperatorName}\", \"{Status}\")'></i>" +
                                    $"</td>";
                            }
                            isFirstOperatorRow = false;
                        }
                     

                        if (isFirstPromotionRow)
                        {
                            html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center\" width=\"200px\">{StartDateTimeString}</td>";
                            html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center\" width=\"200px\">{EndDateTimeString}</td>";
                            html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center text-success\"><a href=\"{urlHelper.Action("leaderboard")}?promotionId={PromotionId}\">View</a></td>";
                            html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center {(HasAnnouncement ? "text-success" : "text-danger")}\">{(HasAnnouncement ? "Linked" : "Unlinked")}</td>";
                            if (hasAdminPermission)
                            {
                                html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center\">" +
                                        $"<button type=\"button\" class=\"btn btn-success btn-lg\" onclick=\"promotion.openEditModal('{Id}')\" >Edit</button>" +
                                        $"<button type=\"button\" class=\"btn btn-danger btn-lg\" onclick=\"promotion.openDeleteModal('{Id}')\">Delete</button>" +
                                        "</td>";
                            }
                            isFirstPromotionRow = false;
                        }

                        html += "</tr>";
                    }
                }
                else
                {
                    // Handle case where operatorModel.CurrencyGroups is null
                    html += "<tr>";
                    if (isFirstPromotionRow)
                    {
                        html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center\">{Name}</td>";
                        html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center\">{PromotionId}</td>";
                        html += $"<td rowspan=\"{promotionRowSpan}\" width=\"150px\">{GetPrizesHtml()}</td>";
                    }

                    html += $"<td rowspan=\"{rowspanDefault}\">{operatorModel.OperatorName}</td>";

                    html += "<td colspan=\"1\" width=\"200px\"></td>";

                    if (hasAdminPermission)
                    {
                        html += $"<td class=\"text-center\" rowspan=\"{rowspanDefault}\">" +
                                    $"<i class=\"icon-edit\" onclick='promotion.showOperatorSetting(\"{Id}\", \"{operatorModel.OperatorId}\", \"{operatorModel.OperatorName}\", \"{Status}\")'></i>" +
                                    $"</td>";
                    }

                    if (isFirstPromotionRow)
                    {
                        html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center\" width=\"200px\">{StartDateTimeString}</td>";
                        html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center\" width=\"200px\">{EndDateTimeString}</td>";
                        html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center text-success\">" +
                            $"<a href=\"{urlHelper.Action("leaderboard")}?promotionId={PromotionId}\">View</a></td>";
                        html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center {(HasAnnouncement ? "text-success" : "text-danger")}\">{(HasAnnouncement ? "Linked" : "Unlinked")}</td>";
                        if (hasAdminPermission)
                        {
                            html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center\">" +
                                    $"<button type=\"button\" class=\"btn btn-success btn-lg\" onclick=\"promotion.openEditModal('{Id}')\" >Edit</button>" +
                                    $"<button type=\"button\" class=\"btn btn-danger btn-lg\" onclick=\"promotion.openDeleteModal('{Id}')\">Delete</button>" +
                                    "</td>";
                        }
                        isFirstPromotionRow = false;
                    }

                    html += "</tr>";
                }
            }

            return html;
        }
    }
}

public class WinningPrizeViewModel
{
    public string Ranking { get; set; }
    public int Value { get; set; }
    public string DisplayValue => Value.ToString();
}

public class OperatorViewModel
{
    public string OperatorId { get; set; }
    public string OperatorName { get; set; }
    public List<CurrencyGroupsViewModel> CurrencyGroups { get; set; }
    public string IncludedSiteNames { get;set; }
}

public class CurrencyGroupsViewModel
{
    public string CurrencyName { get; set; }
    public double SlotGameMinBet { get; set; }
    public double OtherGameMinBet { get; set; }
    public bool Enabled { get; set; }
    public string DisplaySlotGameMinBet => SlotGameMinBet.ToString();
    public string DisplayOtherGameMinBet => OtherGameMinBet.ToString();
}