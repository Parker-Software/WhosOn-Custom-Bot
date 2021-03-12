using Newtonsoft.Json.Linq;
using Parkersoft.WhosOn.BotConnector;
using Parkersoft.WhosOn.BotConnector.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Basic_Rasa_Bot
{
    public class RasaBot : IBot
    {
        public string UniqueName { get; set; }
        public Delegates.BotMessage OnBotMessage { get; set; }


        private readonly string _rasaUrl;

        public RasaBot(string url)
        {
            _rasaUrl = url;
        }

        public void AddProperties(string conversationId, IDictionary<string, string> properties)
        {
            // the rasa session doesn't have the same concept for properties
        }

        public bool CreateConversation(string conversationId, IDictionary<string, string> properties)
        {
            // rasa doesn't require explicit session creation.
            // we could check rasa status here to make sure it is up, and return fals if unavailable.
            return true;
        }

        public bool EndConversation(string conversationId)
        {
            // rasa doesn't require explicit session close
            return true;
        }

        public bool SendMessage(string conversationId, string message)
        {
            Task.Factory.StartNew(() => {
                IBotMessage response;
                try
                {
                    response = GetRasaResponse(conversationId, message).Result;
                }
                catch (Exception ex)
                {
                    // the whoson logging helper will surface the error into the whoson bot log
                    Logging.Log.Error($"an error occurred when sending {message} to {_rasaUrl}", ex);
                    response = new BotNoResponseMessage();
                }
                this.SendBotMessageAsync(conversationId, response);
            });

            return true;
        }

        /// <summary>
        /// call the rasa API to get the chat data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task<IBotMessage> GetRasaResponse(string sender, string message)
        {
            using (var client = new WebClient())
            {
                var content = new JObject();
                content["sender"] = sender;
                content["message"] = message;

                var data = await client.UploadStringTaskAsync(new System.Uri(_rasaUrl), content.ToString());
                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RasaResponse>>(data);

                return ParseMessages(response);
            }
        }

        /// <summary>
        /// convert the message list into a bot message
        /// </summary>
        /// <param name="responses"></param>
        /// <returns></returns>
        private IBotMessage ParseMessages(IList<RasaResponse> responses)
        {
            if (responses.Count == 0)
            {
                return new BotNoResponseMessage();
            }

            if (responses.Count == 1)
            {
                return ParseMessage(responses[0]);
            }

            // if multiple messages are returned we send a bot message group
            // this means that the order of the messages delivery into the chat window is gauranteed
            return new BotMessageGroup() { BotMessages = responses.Select(ParseMessage).ToList() };
        }

        /// <summary>
        /// convert a rasa response into a WhosOn response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private IBotMessage ParseMessage(RasaResponse response)
        {
            if (response.Buttons != null && response.Buttons.Count > 0)
            {
                return new BotButtonMessage() { 
                    Buttons = response.Buttons.Select(x => new BotButtonMessage.ButtonValue(x.Payload, x.Title)).ToList(),
                    Text = response.Text
                };
            }

            if (response.Custom != null)
            {
                // perform custom action processing
                // here is where you will do any additional custom actions
                // we process the transfer event here, as Rasa doesn't have a built in human handoff.
                if (response.Custom.Action == "transfer")
                {
                    return new BotTransferMessage() { Text = response.Custom.Text };
                }
                else
                {
                    Logging.Log(LogLevel.Warning, $"Unknown action type {response.Custom.Action} received from Rasa");
                }
            }

            if (!string.IsNullOrEmpty(response.Text))
            {
                return new BotTextMessage() { Text = response.Text };
            }

            return new BotNoResponseMessage();
        }

    }
}
