using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("taixiu_users")]
    public class BigSmallAccountEntity : MiniGameBaseEntity
    {
        public int TRedPlay { get; set; }
        public double TWinRed { get; set; }
        public int TLostRed { get; set; }
        public int TLineWinRed { get; set; }
        public int TLineLostRed { get; set; }
        public int First { get; set; }
        public int Last { get; set; }
        public int TLineWinRedH { get; set; }
        public int TLineLostRedH { get; set; }
        public int TXuPlay { get; set; }
        public int TWinXu { get; set; }
        public int TLostXu { get; set; }
        public int TLineWinXu { get; set; }
        public int TLineLostXu { get; set; }
        public int TLineWinXuH { get; set; }
        public int TLineLostXuH { get; set; }
        public int CRedPlay { get; set; }
        public int CWinRed { get; set; }
        public int CLostRed { get; set; }
        public int CLineWinRed { get; set; }
        public int CLineLostRed { get; set; }
        public int CLineWinRedH { get; set; }
        public int CLineLostRedH { get; set; }
        public int CXuPlay { get; set; }
        public int CWinXu { get; set; }
        public int CLostXu { get; set; }
        public int CLineWinXu { get; set; }
        public int CLineLostXu { get; set; }
        public int CLineWinXuH { get; set; }
        public int CLineLostXuH { get; set; }
        public string Uid { get; set; }
    }
}
