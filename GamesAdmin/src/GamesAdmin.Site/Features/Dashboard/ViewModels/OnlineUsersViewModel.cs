using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Dashboard.ViewModels
{
    public class OnlineUsersViewModel
    {
        public string Game { get; set; }

        public bool IncludeTestAccount { get; set; }

        public int TotalRealPlayer { get; set; }

        public IEnumerable<OnlineUserViewModel> RealUsers { get; set; }

        public int TotalTestPlayer { get; set; }

        public IEnumerable<OnlineUserViewModel> TestUsers { get; set; }

        public int TotalBots { get; set; }

        public IEnumerable<BotUserViewModel> Bots { get; set; }

        [DisplayName("Operator")]
        public IEnumerable<string> PartnerIds { get; set; }

        public IEnumerable<SelectListItem> PartnerItems { get; set; }
    }

    public class OnlineUserViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public string Character { get; set; }

        public string PartnerId { get; set; }

        public IEnumerable<SelectListItem> PartnerItems { get; set; }

        public string GetPartnerName()
            => PartnerItems?.FirstOrDefault(item => string.Equals(item.Value, PartnerId))?.Text;
    }

    public class BotUserViewModel
    {
        public string Name { get; set; }

        public double Red { get; set; }
    }
}