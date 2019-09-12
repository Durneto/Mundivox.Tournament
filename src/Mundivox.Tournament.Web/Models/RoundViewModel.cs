using Newtonsoft.Json;
using System;

namespace Mundivox.Tournament.Web.Models
{
    public class RoundViewModel : ICloneable
    {
        [JsonProperty("player1")]
        public PlayerViewModel Player1 { get; set; }

        [JsonProperty("player2", NullValueHandling = NullValueHandling.Ignore)]
        public PlayerViewModel Player2 { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
