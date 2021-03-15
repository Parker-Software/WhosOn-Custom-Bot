using Basic_Rasa_Bot;
using Parkersoft.WhosOn.BotConnector;

namespace WhosOnBot
{
    public class RasaBotCreator : IBotCreator
    {
        public Delegates.CreateBot DoCreateBot
        {
            get
            {
                return ((settings) =>
                {
                    // create the bot - we only expect the url of the rasa bot to create it
                    return new RasaBot(settings.Properties["url"]);
                });
            }
        }

        public string BotType => "RasaBot";
    }
}
