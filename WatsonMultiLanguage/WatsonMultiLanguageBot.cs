using Parkersoft.WhosOn.BotConnector;
using Parkersoft.WhosOn.BotConnector.Message;
using Parkersoft.WhosOn.BotConnector.Watson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhosOnBot
{
    public class WatsonMultiLanguageBot : IBot
    {
        public string UniqueName { get; set; }
        public Delegates.BotMessage OnBotMessage { get; set; }

        // Private properties
        private WatsonMultiLanguageBotSettings Settings;
        private Dictionary<string, IBot> Bots;
        private Dictionary<string, string> Conversations;

        public WatsonMultiLanguageBot(WatsonMultiLanguageBotSettings settings)
        {
            Conversations = new Dictionary<string, string>();

            Settings = settings;

            // Create Watson bots
            Bots = new Dictionary<string, IBot>();
            foreach (WatsonMultiLanguageBotLanguageSetting language in Settings.langauges)
            {
                if (Bots.ContainsKey(language.langCode)) continue;

                if (language.iam != null)
                {
                    IBot bot = new WatsonBot(language.iam.apiKey, language.iam.endpoint, language.iam.assistantId);
                    bot.OnBotMessage = (b, c, m) => this.OnBotMessage?.Invoke(b, c, m);
                    Bots.Add(language.langCode, bot);
                }
                else
                {
                    IBot bot = new WatsonBot(language.serviceCredentials.username, language.serviceCredentials.password, language.serviceCredentials.endpoint, language.serviceCredentials.assistantId);
                    bot.OnBotMessage = (b, c, m) => this.OnBotMessage?.Invoke(b, c, m);
                    Bots.Add(language.langCode, bot);
                }
            }
        }

        public void AddProperties(string conversationId, IDictionary<string, string> properties)
        {

        }

        public bool CreateConversation(string conversationId, IDictionary<string, string> properties)
        {
            // Save which bot to use
            if (Conversations.ContainsKey(conversationId))
                Conversations.Remove(conversationId);
            if (properties.ContainsKey("Language"))
                Conversations.Add(conversationId, properties["Langauge"]);
            else
                Conversations.Add(conversationId, "en");

            return GetBot(conversationId).CreateConversation(conversationId, properties);
        }

        public bool EndConversation(string conversationId)
        {
            return GetBot(conversationId).EndConversation(conversationId);
        }

        public bool SendMessage(string conversationId, string message)
        {
            return GetBot(conversationId).SendMessage(conversationId, message);
        }

        private IBot GetBot(string conversationId)
        {
            if (Bots.ContainsKey(Conversations[conversationId]))
                return Bots[Conversations[conversationId]];
            // Bot cant be found for that language, return the first bot
            return Bots.Select(a => a.Value).FirstOrDefault();
        }
    }
}
