namespace GamesAdmin.Core.Models
{
    public class AccountInfo
    {
        public AccountInfo(string name, double red, bool isBot, byte spins)
        {
            Name = name;
            Red = red;
            IsBot = isBot;
            Spins = spins;
        }

        public double Red { get; }

        public string Name { get; }

        public bool IsBot { get; }

        public byte Spins { get; set; }
    }
}
