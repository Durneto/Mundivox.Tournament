using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mundivox.Tournament.Web.Models
{
    public class BracketsViewModel
    {
        [JsonProperty("titles")]
        public List<string> Titles { get; set; } = new List<string>();

        [JsonProperty("rounds")]
        public List<List<RoundViewModel>> Rounds { get; set; } = new List<List<RoundViewModel>>();
    }
}
