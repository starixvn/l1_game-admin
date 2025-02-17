using System.Collections;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.OperateSettings.Responses
{
    public class ImportGameSettingResponse
    {
        public bool IsSuccessfully { get; set; }

        public string Message { get; set; }

        public IDictionary<int, bool> ImportResult { get; set; } = new Dictionary<int, bool>();
    }
}
