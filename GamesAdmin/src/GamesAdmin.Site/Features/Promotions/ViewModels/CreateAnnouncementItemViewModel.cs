using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class CreateAnnouncementItemViewModel
    {
        public string Id { get; set; }

        [JsonConverter(typeof(NotificationDisplayKindConverter))]
        public NotificationDisplayKind DisplayKind { get; set; }
        public bool Enabled { get; set; }
        public Dictionary<string, string> Title { get; set; } = Language.EmptyLanguages;
        public Dictionary<string, string> Messages { get; set; } = Language.EmptyLanguages;
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public DateTime EndTime { get; set; } = DateTime.UtcNow;
        public string PromotionId { get; set; }
        public bool IsHappyHoursAnnouncement => DisplayKind?.Value == NotificationDisplayKind.Promotion.Value;

        public IEnumerable<PromotionModel> Promotions { get; set; }

        public IEnumerable<PromotionConfigViewModel> PromotionCampaigns { get; set; }

        public IEnumerable<SelectListItem> PromotionListItem => Promotions?.Select(promotion => new SelectListItem(GetDisplayPromotion(promotion), promotion.PromotionId))
                                                                            .Prepend(new SelectListItem("None", string.Empty));

        public IEnumerable<SelectListItem> PromotionCampaignListItem => PromotionCampaigns?.Select(promotionCampaign => new SelectListItem(GetDisplayPromotionCampaign(promotionCampaign), promotionCampaign.Id))
                                                                            .Prepend(new SelectListItem("None", string.Empty));

        public IEnumerable<SelectListItem> PromotionTypeList = new List<SelectListItem>
        {
            new SelectListItem { Value = NotificationDisplayKind.PromotionValue, Text = NotificationDisplayKind.Promotion.DisplayName },

            new SelectListItem { Value = NotificationDisplayKind.PromotionCampaignValue, Text = NotificationDisplayKind.PromotionCampaign.DisplayName }
        };

        public string GetDisplayPromotion(PromotionModel promotion)
        {
            var gameName = Game.FromGameId(promotion.GameId).DisplayName;
            return promotion.IsDaily ?
                $"{gameName} - From {promotion.StartDateTime.ToDateFormat()}" +
                $" to {promotion.EndDateTime.ToDateFormat()}, {promotion.StartDateTime.ToTimeFormat()} - {promotion.EndDateTime.ToTimeFormat()}"
                : $"{gameName} - From {promotion.StartDateTime.ToString(DateTimeExtensions.DateTimeFormat)}" +
                $" to {promotion.EndDateTime.ToString(DateTimeExtensions.DateTimeFormat)}";
        }

        public string GetDisplayPromotionCampaign(PromotionConfigViewModel promotionCampaign)
        {
            return $"{promotionCampaign.Name} - From {promotionCampaign.StartDateTime.ToString(DateTimeExtensions.DateTimeFormat)}" +
                $" to {promotionCampaign.EndDateTime.ToString(DateTimeExtensions.DateTimeFormat)}";
        }

        public void ConvertToTimeZone(TimeZoneInfo timeZone)
        {
            if (Id == null)
            {
                StartTime = TimeZoneInfo.ConvertTimeFromUtc(StartTime, timeZone);
                EndTime = TimeZoneInfo.ConvertTimeFromUtc(EndTime, timeZone);
            }
        }
    }

    public class NotificationDisplayKindConverter : JsonConverter<NotificationDisplayKind>
    {
        public override NotificationDisplayKind Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            return value switch
            {
                NotificationDisplayKind.MarqueeValue => NotificationDisplayKind.Marquee,
                NotificationDisplayKind.DisabledValue => NotificationDisplayKind.DisabledGame,
                NotificationDisplayKind.PromotionValue => NotificationDisplayKind.Promotion,
                NotificationDisplayKind.PromotionCampaignValue => NotificationDisplayKind.PromotionCampaign,
                _ => throw new JsonException($"Invalid NotificationDisplayKind value: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, NotificationDisplayKind value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}