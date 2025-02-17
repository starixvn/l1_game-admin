using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GamesAdmin.Site.Features.OperateSettings.Requests
{
    public class ExportGameSettingRequest
    {
        [JsonPropertyName("gameIds")]
        public IList<int> GameIds { get; set; }
    }
}
