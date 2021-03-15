using Newtonsoft.Json;

namespace Basic_Rasa_Bot
{
    /// <summary>
    /// class that defines how your custom model works
    /// you need to stick to this when you return data from rasa, or use a more free property bag notation
    /// </summary>
    class RasaCustomModel
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        public string Text { get; set; }
    }
}
