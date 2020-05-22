using System;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.VietlottKeno.ViewModels
{
    public class ViewlottKenoRoundViewModel
    {
        public DateTime Time { get; set; }

        public string RoundNo { get; set; }

        public IEnumerable<byte> Results { get; set; }
    }
}