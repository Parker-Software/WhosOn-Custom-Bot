using Newtonsoft.Json;

namespace Basic_Rasa_Bot
{
    class RasaButton
    {
        [JsonProperty("payload")]
        public string Payload { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
