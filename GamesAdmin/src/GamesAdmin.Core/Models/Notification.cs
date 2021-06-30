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

        public string Id { get; set; }

        public string Message { get; set; }

        public NotificationDisplayKind DisplayKind { get; set; }

        public string GameName { get; set; }

        public Dictionary<string, string> Messages { get; set; }
    }
}