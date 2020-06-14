using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class CreateProVietKenoRound
    {
        public DateTime Time { get; set; }

        public long Number { get; set; }

        public IEnumerable<byte> Results { get; set; }
    }
}