namespace GamesAdmin.Core.Models.GameConfigs
{
    public class Choice
    {
        public Choice() { }

        public Choice(string key, double odds, string name)
        {
            Key = key;
            Odds = odds;
            Name = name;
        }

        public string Key { get; set; }

        public string Name { get; set; }

        public double Odds { get; set; }
    }
}
