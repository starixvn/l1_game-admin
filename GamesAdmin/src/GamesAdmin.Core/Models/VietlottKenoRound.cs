using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class VietlottKenoRound
    {
        public DateTime Time { get; set; }

        public string RoundNo { get; set; }

        public IEnumerable<byte> Results { get; set; }
    }
}