using System;

namespace GamesAdmin.Core.Models.Spins
{
    public class SpinLog
    {
        public SpinLog(int memberId, string key, int spinCount, string source, DateTime time, DateTime expiryDate)
        {
            MemberId = memberId;
            Key = key;
            SpinCount = spinCount;
            Source = source;
            Time = time;
            ExpiryDate = expiryDate;
        }

        public int MemberId { get; set; }

        public string Key { get; }

        public int SpinCount { get; set; }

        public string Source { get; set; }

        public DateTime Time { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}
