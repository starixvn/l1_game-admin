using System;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Api._Shared.Configurations
{
    public class CloudSettings
    {
        public bool IsCloudService { get; set; }

        public bool EnableCloud { get; set; }

        public string CloudAdminApiUrl { get; set; }

        public string ProxyUrl { get; set; }

        public string GameManagementUrl { get; set; }

        public IEnumerable<int> CloudGameIds { get; set; }

        public bool IsCloudGame(int gameTypeId) => CloudGameIds != null && CloudGameIds.Contains(gameTypeId);

        public bool ShouldCallCloudService(int gameTypeId)
            => !IsCloudService && IsCloudGame(gameTypeId);
    }
}