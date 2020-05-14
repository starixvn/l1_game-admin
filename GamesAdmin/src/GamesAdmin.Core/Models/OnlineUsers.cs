using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class OnlineUsers
    {
        public OnlineUsers() 
        {
            RealUsers = new List<OnlineUser>();
            UusUsers = new List<OnlineUser>();
            Bots = new List<BotUser>();
        }

        public int TotalReal { get; set; }

        public IEnumerable<OnlineUser> RealUsers { get; set; }

        public int TotalUUS { get; set; }

        public IEnumerable<OnlineUser> UusUsers { get; set; }

        public int TotalBots { get; set; }

        public IEnumerable<BotUser> Bots { get; set; }
    }

    public class OnlineUser 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public string Character { get; set; }
    }

    public class BotUser
    {
        public string Name { get; set; }

        public double Red { get; set; }
    }
}