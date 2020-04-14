using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("taixiu_users")]
    public class BigSmallAccountEntity : MiniGameBaseEntity
    {
        public int tRedPlay { get; set; }
        public double tWinRed { get; set; }
        public int tLostRed { get; set; }
        public int tLineWinRed { get; set; }
        public int tLineLostRed { get; set; }
        public int first { get; set; }
        public int last { get; set; }
        public int tLineWinRedH { get; set; }
        public int tLineLostRedH { get; set; }
        public int tXuPlay { get; set; }
        public int tWinXu { get; set; }
        public int tLostXu { get; set; }
        public int tLineWinXu { get; set; }
        public int tLineLostXu { get; set; }
        public int tLineWinXuH { get; set; }
        public int tLineLostXuH { get; set; }
        public int cRedPlay { get; set; }
        public int cWinRed { get; set; }
        public int cLostRed { get; set; }
        public int cLineWinRed { get; set; }
        public int cLineLostRed { get; set; }
        public int cLineWinRedH { get; set; }
        public int cLineLostRedH { get; set; }
        public int cXuPlay { get; set; }
        public int cWinXu { get; set; }
        public int cLostXu { get; set; }
        public int cLineWinXu { get; set; }
        public int cLineLostXu { get; set; }
        public int cLineWinXuH { get; set; }
        public int cLineLostXuH { get; set; }
        public string uid { get; set; }
    }
}
