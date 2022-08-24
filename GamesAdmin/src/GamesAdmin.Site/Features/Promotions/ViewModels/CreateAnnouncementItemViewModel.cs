using System;
using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class CreateAnnouncementItemViewModel
    {
        public string Id { get; set; }
        public NotificationDisplayKind DisplayKind => NotificationDisplayKind.Promotion;
        public bool Enabled { get; set; }
        public Dictionary<string, string> Title { get; set; } = Language.EmptyLanguages;
        public Dictionary<string, string> Messages { get; set; } = Language.EmptyLanguages;
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now;
        public string PromotionId { get; set; }
        public IEnumerable<PromotionModel> Promotions { get; set; }

        public IEnumerable<SelectListItem> PromotionListItem => Promotions?.Select(promotion => new SelectListItem(GetDisplayPromotion(promotion), promotion.PromotionId))
                                                                            .Prepend(new SelectListItem("None", string.Empty));

        public string GetDisplayPromotion(PromotionModel promotion)
        {
            var gameName = Game.FromGameId(promotion.GameId).DisplayName;
            return promotion.IsDaily ?
                $"{gameName} - From {promotion.StartDateTime.ToDateFormat()}" +
                $" to {promotion.EndDateTime.ToDateFormat()}, {promotion.StartDateTime.ToTimeFormat()} - {promotion.EndDateTime.ToTimeFormat()}"
                : $"{gameName} - From {promotion.StartDateTime.ToString(DateTimeExtensions.DateTimeFormat)}" +
                $" to {promotion.EndDateTime.ToString(DateTimeExtensions.DateTimeFormat)}";
        }
    }
}