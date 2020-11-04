namespace GamesAdmin.Site.Features.Bot.ViewModels
{
    public class DeactivateViewModel
    {
        public DeactivateViewModel(int total)
        {
            Total = total;
        }

        public int Total { get; }
    }
}
