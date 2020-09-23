using Parkersoft.WhosOn.BotConnector;
using Parkersoft.WhosOn.BotConnector.Message;
using System.Collections.Generic;

namespace WhosOnBot
{
    public class CustomBot : IBot
    {
        public string UniqueName { get; set; }
        public Delegates.BotMessage OnBotMessage { get; set; }

        public void AddProperties(string conversationId, IDictionary<string, string> properties)
        {
            return;
        }

        public bool CreateConversation(string conversationId, IDictionary<string, string> properties)
        {
            return true;
        }

        public bool EndConversation(string conversationId)
        {
            return true;
        }

        public bool SendMessage(string conversationId, string message)
        {
            var charArr = message.ToCharArray();
            System.Array.Reverse(charArr);
            this.SendBotMessageAsync(conversationId, new BotTextMessage() { Text = new string(charArr) });
            return true;
        }
    }
}
