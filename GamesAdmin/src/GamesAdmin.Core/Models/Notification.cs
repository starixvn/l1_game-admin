using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using Newtonsoft.Json;

namespace GamesAdmin.Core.Models
{
    public class Notification
    {
        public Notification()
        {
        }

        [JsonConstructor]
        public Notification(
            string id,
            string message,
            NotificationDisplayKind displayKind,
            Dictionary<string, string> messages,
            string gameName = ""
            )
        {
            Id = id;
            Message = message;
            DisplayKind = displayKind;
            Messages = messages;
            GameName = gameName;
        }

        public Notification(
           string id,
           string message,
           NotificationDisplayKind displayKind,
           Dictionary<string, string> messages,
           Dictionary<string, string> title,
           DateTime? startTime,
           DateTime? endTime,
           string promotionId,
           bool? enabled,
           int? gameId,
           string gameName = ""
           )
        {
            Id = id;
            Message = message;
            DisplayKind = displayKind;
            Messages = messages;
            GameName = gameName;
            Title = title;
            StartTime = startTime;
            EndTime = endTime;
            PromotionId = promotionId;
            Enabled = enabled;
            GameId = gameId;
        }

        public string Id { get; set; }

        public string Message { get; set; }

        public NotificationDisplayKind DisplayKind { get; set; }

        public string GameName { get; set; }

        public Dictionary<string, string> Messages { get; set; }

        public Dictionary<string, string> Title { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string PromotionId { get; set; }

        public bool? Enabled { get; set; }

        public int? GameId { get; set; }
    }
}