using System;

namespace GamesAdmin.Core.Models.Spins
{
    public class SpinResult
    {
        public SpinResult(string uid, string name, int memberId, byte result, DateTime time) 
        {
            Uid = uid;
            Name = name;
            MemberId = memberId;
            Result = result;
            Time = time;
        }

        public string Uid { get; set; }

        public string Name { get; set; }

        public int MemberId { get; set; }

        public byte Result { get; set; }

        public DateTime Time { get; set; }
    }
}
