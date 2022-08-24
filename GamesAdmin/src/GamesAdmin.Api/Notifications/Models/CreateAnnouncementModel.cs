using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Api.Notifications.Models
{
    public class CreateAnnouncementModel
    {
        public CreateAnnouncementModel(Notification notification)
        {
            DisplayKind = notification.DisplayKind;
            GameName = notification.GameName;
            Messages = notification.Messages;
            Title = notification.Title;
            StartTime = notification.StartTime ?? DateTime.Now;
            EndTime = notification.EndTime ?? DateTime.Now;
            PromotionId = notification.PromotionId;
            Enabled = notification.Enabled ?? false;
            GameId = notification.GameId;
        }

        public NotificationDisplayKind DisplayKind { get; }

        public string GameName { get; }

        public Dictionary<string, string> Messages { get; }

        public Dictionary<string, string> Title { get; }

        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public string PromotionId { get; }

        public bool Enabled { get; }

        public byte? GameId { get; }
    }
}