using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Dashboard.ViewModels
{
    public class OnlineUsersViewModel
    {
        public List<SelectListItem> GameTypeItems { get; set; }

        public string Game { get; set; }

        public bool IncludeUUS { get; set; }

        public int TotalReal { get; set; }

        public IEnumerable<OnlineUserViewModel> RealUsers { get; set; }

        public int TotalUUS { get; set; }

        public IEnumerable<OnlineUserViewModel> UusUsers { get; set; }

       
        public int TotalBots { get; set; }

        public IEnumerable<BotUserViewModel> Bots { get; set; }
    }

    public class OnlineUserViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public string Character { get; set; }
    }

    public class BotUserViewModel
    {
        public string Name { get; set; }

        public double Red { get; set; }
    }
}