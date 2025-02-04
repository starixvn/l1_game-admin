using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class UpdateInternalUUSTestResult
    {
        public UpdateInternalUUSTestResult(bool success, List<string> invalidAccounts)
        {
            Success = success;
            InvalidAccounts = invalidAccounts;
        }
    
        public bool Success { get; set; }
        public List<string> InvalidAccounts { get; set; } = new List<string>();
    }
}
