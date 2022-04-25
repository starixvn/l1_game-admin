using System.Collections.Generic;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class UpdateOddsResultViewModel
    {
        public UpdateOddsResultViewModel()
        {
            FailedRanges = new List<FailedRangeViewModel>();
        }

        public List<FailedRangeViewModel> FailedRanges { get; set; }
    }

    public class FailedRangeViewModel
    {
        public FailedRangeViewModel() { }

        public FailedRangeViewModel(int icon, int oddsIndex, string message)
        {
            Icon = icon;
            OddsIndex = oddsIndex;
            Message = message;
        }

        public int Icon { get; set; }

        public int OddsIndex { get; set; }

        public string Message { get; set; }
    }
}
