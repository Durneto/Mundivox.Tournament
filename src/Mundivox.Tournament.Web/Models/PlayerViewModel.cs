using Newtonsoft.Json;

namespace Mundivox.Tournament.Web.Models
{
    public class PlayerViewModel
    {
        [JsonProperty("ID")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("winner")]
        public bool Winner { get; set; }

        [JsonProperty("onclick")]
        public string Onclick { get; set; }
    }
}
