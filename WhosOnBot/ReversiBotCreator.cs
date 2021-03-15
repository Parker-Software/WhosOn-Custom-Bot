using Parkersoft.WhosOn.BotConnector;

namespace WhosOnBot
{
    public class ReversiBotCreator : IBotCreator
    {
        public Delegates.CreateBot DoCreateBot
        {
            get
            {
                return ((settings) =>
                {
                    return new ReversiBot();
                });
            }
        }

        public string BotType => "Reversi";
    }
}
