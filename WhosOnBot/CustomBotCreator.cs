using Parkersoft.WhosOn.BotConnector;

namespace WhosOnBot
{
    public class CustomBotCreator : IBotCreator
    {
        public Delegates.CreateBot DoCreateBot
        {
            get
            {
                return ((x) =>
                {
                    return new CustomBot();
                });
            }
        }

        public string BotType => "CustomBot";
    }
}
