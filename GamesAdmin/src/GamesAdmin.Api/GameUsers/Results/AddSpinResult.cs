using GamesAdmin.Api._Shared;

namespace GamesAdmin.Api.GameUsers.Results
{
    public class AddSpinResult : BaseResult
    {
        public string Key { get; set; }

        public string ObCustId { get; set; }
    }
}
