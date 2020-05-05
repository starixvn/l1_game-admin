using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class OnlineUsers
    {
        public int TotalReal { get; set; }

        public IEnumerable<OnlineUser> RealUsers { get; set; }

        public int TotalUUS { get; set; }

        public IEnumerable<OnlineUser> UusUsers { get; set; }
    }

    public class OnlineUser 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public string Character { get; set; }
    }
}