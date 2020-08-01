using Newtonsoft.Json;

namespace GamesAdmin.Workers.VietlottKeno.DTOs
{
    public class KenoResultDto
    {
        [JsonProperty("kq")]
        public string[] Results { get; set; }

        [JsonProperty("ky")]
        public long RoundId { get; set; }

        public string Date { get; set; }
    }
}
