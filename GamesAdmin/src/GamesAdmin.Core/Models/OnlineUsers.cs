using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class OnlineUsers
    {
        public OnlineUsers() 
        {
            RealUsers = new List<OnlineUser>();
            TestUsers = new List<OnlineUser>();
            Bots = new List<BotUser>();
        }

        public int TotalRealPlayer { get; set; }

        public IEnumerable<OnlineUser> RealUsers { get; set; }

        public int TotalTestPlayer { get; set; }

        public IEnumerable<OnlineUser> TestUsers { get; set; }

        public int TotalBots { get; set; }

        public IEnumerable<BotUser> Bots { get; set; }
    }

    public class OnlineUser 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public string Character { get; set; }

        public string PartnerId { get; set; }
    }

    public class BotUser
    {
        public string Name { get; set; }

        public double Red { get; set; }
    }
}