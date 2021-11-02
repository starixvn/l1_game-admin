using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Dashboard.ViewModels
{
    public class OnlineUsersViewModel
    {
        public List<SelectListItem> GameTypeItems { get; set; }

        public string Game { get; set; }

        public bool IncludeTestAccount { get; set; }

        public int TotalRealPlayer { get; set; }

        public IEnumerable<OnlineUserViewModel> RealUsers { get; set; }

        public int TotalTestPlayer { get; set; }

        public IEnumerable<OnlineUserViewModel> TestUsers { get; set; }

        public int TotalBots { get; set; }

        public IEnumerable<BotUserViewModel> Bots { get; set; }

        public IEnumerable<string> PartnerIds { get; set; }

        public IEnumerable<SelectListItem> PartnerItems { get; set; }
    }

    public class OnlineUserViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public string Character { get; set; }

        public string PartnerId { get; set; }
    }

    public class BotUserViewModel
    {
        public string Name { get; set; }

        public double Red { get; set; }
    }
}