using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Api._Shared.Configurations
{
    public class CloudSettings
    {
        public bool IsCloudService { get; set; }

        public string CloudAdminApiUrl { get; set; }

        public IEnumerable<int> SeparatedGameIds { get; set; } = new List<int>();

        public bool ShouldCallCloudService(int gameTypeId)
            => !IsCloudService && SeparatedGameIds.Contains(gameTypeId);
    }
}