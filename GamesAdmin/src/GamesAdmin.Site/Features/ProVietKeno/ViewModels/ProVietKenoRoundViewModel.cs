using System;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.ProVietKeno.ViewModels
{
    public class ProVietKenoRoundViewModel
    {
        public DateTime Time { get; set; }

        public string RoundNo { get; set; }

        public IEnumerable<byte> Result { get; set; }

        public bool? Big { get; set; }
        public bool? Small { get; set; }
        public bool? Odd { get; set; }
        public bool? Even { get; set; }
        public bool? Gold { get; set; }
        public bool? Wood { get; set; }
        public bool? Water { get; set; }
        public bool? Fire { get; set; }
        public bool? Earth { get; set; }
        public bool Settled { get; internal set; }
    }
}