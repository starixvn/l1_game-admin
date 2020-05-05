using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Dashboard.ViewModels
{
    public class OnlineUsersViewModel
    {
        public bool IncludeUUS { get; set; }

        public int TotalReal { get; set; }

        public IEnumerable<OnlineUserViewModel> RealUsers { get; set; }

        public int TotalUUS { get; set; }

        public IEnumerable<OnlineUserViewModel> UusUsers { get; set; }
    }

    public class OnlineUserViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public string Character { get; set; }
    }
}