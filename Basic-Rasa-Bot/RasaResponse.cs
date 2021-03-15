using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;

namespace Basic_Rasa_Bot
{
    class RasaResponse
    {
        [JsonProperty("recipient_id")]
        public string RecipientId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("buttons")]
        public IList<RasaButton> Buttons { get; set; }

        [JsonProperty("custom")]
        public RasaCustomModel Custom { get; set; }
    }
}
