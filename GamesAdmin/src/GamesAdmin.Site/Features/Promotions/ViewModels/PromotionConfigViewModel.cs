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
        public bool HasAnnouncement { get; set; }

        public string Status { get; set; }
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

            if (Operators?.Count() > 0)
            {
                var promotionRowSpan = Operators?.Sum(o => o.CurrencyGroups?.Count);
                foreach (var operatorModel in Operators)
                {
                    var operatorRowSpan = operatorModel?.CurrencyGroups?.Count;
                    bool isFirstOperatorRow = true;

                    if (operatorModel?.CurrencyGroups?.Count() > 0)
                    {
                        foreach (var currencyGroup in operatorModel.CurrencyGroups)
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
                                isFirstOperatorRow = false;
                            }

                            html += $"<td width=\"200px\">" +
                                    $"<div><strong>{currencyGroup.CurrencyName}</strong></div>" +
                                    $"<div>Slot games: {currencyGroup.DisplaySlotGameMinBet}</div>" +
                                    $"<div>Other games: {currencyGroup.DisplayOtherGameMinBet}</div>" +
                                    "</td>";

                            html += $"<td class=\"text-center\">" +
                                    $"<i class=\"{currencyGroup.StatusIcon}\"></i>" +
                                    "</td>";

                            if (hasAdminPermission)
                            {
                                html += "<td class=\"text-center\"><i class=\"icon-edit\"></i></td>";
                            }

                            if (isFirstPromotionRow)
                            {
                                html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center\" width=\"200px\">{StartDateTimeString}</td>";
                                html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center\" width=\"200px\">{EndDateTimeString}</td>";
                                html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center text-success\"><a href=\"{urlHelper.Action("leaderboard")}\">View</a></td>";
                                html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center {(HasAnnouncement ? "text-success" : "text-danger")}\">{(HasAnnouncement ? "Linked" : "Unlinked")}</td>";
                                if (hasAdminPermission)
                                {
                                    html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center\">" +
                                            $"<a type=\"button\" class=\"btn btn-success btn-lg\" data-ajax=\"true\" data-ajax-mode=\"replace\" data-ajax-update=\"#promotion-modal .modal-content\" data-ajax-success=\"promotion.openUpdatePromotionForm\" asp-route-id=\"{PromotionId}\" asp-action=\"Update\">  Edit  </a>" +
                                            $"<button type=\"button\" class=\"btn btn-danger btn-lg\" onclick=\"promotion.openDeleteModal('{PromotionId}')\">Delete</button>" +
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

                        html += "<td colspan=\"2\" width=\"200px\">No currency groups available</td>";

                        if (hasAdminPermission)
                        {
                            html += "<td class=\"text-center\"><i class=\"icon-edit\"></i></td>";
                        }

                        if (isFirstPromotionRow)
                        {
                            html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center\" width=\"200px\">{StartDateTimeString}</td>";
                            html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center\" width=\"200px\">{EndDateTimeString}</td>";
                            html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center text-success\"><a href=\"{urlHelper.Action("leaderboard")}\">View</a></td>";
                            html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center {(HasAnnouncement ? "text-success" : "text-danger")}\">{(HasAnnouncement ? "Linked" : "Unlinked")}</td>";
                            if (hasAdminPermission)
                            {
                                html += $"<td rowspan=\"{promotionRowSpan}\" class=\"text-center\">" +
                                        $"<a type=\"button\" class=\"btn btn-success btn-lg\" data-ajax=\"true\" data-ajax-mode=\"replace\" data-ajax-update=\"#promotion-modal .modal-content\" data-ajax-success=\"promotion.openUpdatePromotionForm\" asp-route-id=\"{PromotionId}\" asp-action=\"Update\">  Edit  </a>" +
                                        $"<button type=\"button\" class=\"btn btn-danger btn-lg\" onclick=\"promotion.openDeleteModal('{PromotionId}')\">Delete</button>" +
                                        "</td>";
                            }
                            isFirstPromotionRow = false;
                        }

                        html += "</tr>";
                    }
                }
            }
            else
            {
                // Handle case where Operators is null
                html += "<tr>";
                html += $"<td rowspan=\"{rowspanDefault}\" class=\"text-center\">{Name}</td>";
                html += $"<td rowspan=\"{rowspanDefault}\" class=\"text-center\">{PromotionId}</td>";
                html += $"<td rowspan=\"{rowspanDefault}\" width=\"150px\">{GetPrizesHtml()}</td>";
                html += "<td colspan=\"3\" class=\"text-center\">No operators available</td>";
                if (hasAdminPermission)
                {
                    html += "<td class=\"text-center\"><i class=\"icon-edit\"></i></td>";
                }
                html += $"<td rowspan=\"{rowspanDefault}\" class=\"text-center\" width=\"200px\">{StartDateTimeString}</td>";
                html += $"<td rowspan=\"{rowspanDefault}\" class=\"text-center\" width=\"200px\">{EndDateTimeString}</td>";
                html += $"<td rowspan=\"{rowspanDefault}\" class=\"text-center text-success\"><a href=\"{urlHelper.Action("leaderboard")}\">View</a></td>";
                html += $"<td rowspan=\"{rowspanDefault}\" class=\"text-center {(HasAnnouncement ? "text-success" : "text-danger")}\">{(HasAnnouncement ? "Linked" : "Unlinked")}</td>";
                if (hasAdminPermission)
                {
                    html += $"<td rowspan=\"{rowspanDefault}\" class=\"text-center\">" +
                            $"<a type=\"button\" class=\"btn btn-success btn-lg\" data-ajax=\"true\" data-ajax-mode=\"replace\" data-ajax-update=\"#promotion-modal .modal-content\" data-ajax-success=\"promotion.openUpdatePromotionForm\" asp-route-id=\"{PromotionId}\" asp-action=\"Update\">  Edit  </a>" +
                            $"<button type=\"button\" class=\"btn btn-danger btn-lg\" onclick=\"promotion.openDeleteModal('{PromotionId}')\">Delete</button>" +
                            "</td>";
                }
                html += "</tr>";
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
}

public class CurrencyGroupsViewModel
{
    public string CurrencyName { get; set; }
    public double SlotGameMinBet { get; set; }
    public double OtherGameMinBet { get; set; }
    public bool Enabled { get; set; }
    public string DisplaySlotGameMinBet => SlotGameMinBet.ToString();
    public string DisplayOtherGameMinBet => OtherGameMinBet.ToString();
    public string StatusIcon => Enabled ? "icon-check" : "icon-cancel";
}
