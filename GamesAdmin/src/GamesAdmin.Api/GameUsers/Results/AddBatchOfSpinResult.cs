using System.Collections.Generic;
using GamesAdmin.Api._Shared;

namespace GamesAdmin.Api.GameUsers.Results
{
    public class AddBatchOfSpinResult : BaseResult
    {
        public AddBatchOfSpinResult(IList<AddMemberSpinResult> errorSpins) 
        {
            ErrorCode = errorSpins?.Count > 0 ? "1000" : "0";
            ErrorDescription = errorSpins?.Count > 0 ? "Unspecified Error." : string.Empty;
        }

        public IList<AddMemberSpinResult> ErrorSpins { get; set; }        
    }

    public class AddMemberSpinResult : BaseResult
    {
        public string Key { get; set; }

        public string ObCustId { get; set; }
    }
}
