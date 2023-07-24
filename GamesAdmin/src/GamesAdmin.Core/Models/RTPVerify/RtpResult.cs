namespace GamesAdmin.Core.Models
{
    using System.Collections.Generic;

    public class RtpItem
    {
        public string RTP { get; set; }
        public int[] SymbolFrequency { get; set; }
    }

    public class RtpResult
    {
        public List<List<RtpItem>> Result { get; set; }
    }
}
