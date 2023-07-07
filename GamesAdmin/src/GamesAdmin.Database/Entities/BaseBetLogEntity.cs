namespace GamesAdmin.Database.Entities
{
    public class BaseBetLogEntity : BaseBetEntity
    {
        public string Choice { get; set; }
        public decimal Amount { get; set; }
    }
}