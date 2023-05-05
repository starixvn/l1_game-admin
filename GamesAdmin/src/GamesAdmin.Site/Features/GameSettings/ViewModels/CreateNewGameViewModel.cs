using GamesAdmin.Site.Features._Shared;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class CreateNewGameViewModel : ViewModelBase
    {
        public AddViewModel AddViewModel;

        public AddSlotGameViewModel AddSlotGameViewModel;

        public CreateNewGameViewModel() { }

        public CreateNewGameViewModel(
            AddViewModel addViewModel,
            AddSlotGameViewModel addSlotGameViewModel)
        {
            AddViewModel = addViewModel;
            AddSlotGameViewModel = addSlotGameViewModel;
        }
    }
}