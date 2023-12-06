using System;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Api._Shared.Configurations
{
    public class CloudSettings
    {
        public bool IsCloudService { get; set; }

        public string CloudAdminApiUrl { get; set; }

        public IEnumerable<int> CloudGameIds { get; set; }

        public bool ShouldCallCloudService(int gameTypeId)
            => !IsCloudService && CloudGameIds != null && CloudGameIds.Contains(gameTypeId);
    }
}