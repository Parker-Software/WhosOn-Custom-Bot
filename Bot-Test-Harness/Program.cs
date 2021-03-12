using Parkersoft.WhosOn.BotConnector;
using Parkersoft.WhosOn.BotConnector.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhosOnBot;

namespace Bot_Test_Harness
{
    /// <summary>
    /// test framework to let you try out your bot outside of WhosOn.
    /// </summary>
    class Program
    {
        static IBot _selectedBot = null;

        public static Delegates.BotMessage _botMessageDelegate { get; private set; }

        private static bool _waitingForResponse = false;

        static void Main(string[] args)
        {

            _botMessageDelegate = (bot, chatuid, message) => {
                Console.ForegroundColor = ConsoleColor.Yellow;
                PrintToConsole(message);
                Console.ResetColor();
                _waitingForResponse = false;
            };

            Console.WriteLine("1. Rasa Bot");
            Console.WriteLine("2. Reversi Bot");


            var key = Console.ReadKey();

            if (key.KeyChar == '1')
            {
                _selectedBot = CreateRasaBot();
            }
            else if (key.KeyChar == '2')
            {
                _selectedBot = CreateReversiBot();
            }
            else
            {
                return;
            }

            Console.WriteLine($"bot created {_selectedBot}");

            string convoId = Guid.NewGuid().ToString();
            _selectedBot.CreateConversation(convoId, new Dictionary<string, string>());

            bool exit = false;

            Console.WriteLine("Say bye to end the conversation. otherwise, just chat away.");
            _waitingForResponse = true;
            do
            {
                var input = Console.ReadLine();
                if ("bye".Equals(input))
                {
                    exit = true;
                }
                else
                {
                    _waitingForResponse = true;
                    _selectedBot.SendMessage(convoId, input);
                }

                System.Threading.Thread.Sleep(100);
            } while (!exit);


            while (_waitingForResponse)
            {
                Task.Yield();
            }


            Console.WriteLine($"bye");

        }

        static IBot CreateReversiBot()
        {
            var creator = new ReversiBotCreator();
            var bot = creator.DoCreateBot(new BotSettings()
            {
                BotType = "Reversi",
                Properties = new Dictionary<string, string>()
            });
            bot.OnBotMessage = _botMessageDelegate;

            return bot;
        }

        static IBot CreateRasaBot()
        {
            var props = new Dictionary<string, string>();
            props["url"] = "http://13.74.32.44/webhooks/rest/webhook";

            var creator = new RasaBotCreator();
            var bot = creator.DoCreateBot(new BotSettings()
            {
                BotType = "RasaBot",
                Properties = props
            });
            bot.OnBotMessage = _botMessageDelegate;

            return bot;
        }


        static void PrintToConsole(IBotMessage message)
        {
            switch (message)
            {
                case BotMessageGroup bmg:
                    foreach (var subMessage in ((BotMessageGroup)message).BotMessages)
                    {
                        PrintToConsole(subMessage);
                    }
                    break;
                case BotTextMessage btm:
                    Console.WriteLine(btm.Text);
                    break;
                case BotButtonMessage bbm:
                    Console.WriteLine($"buttons {bbm.Text}");
                    bbm.Buttons.ToList().ForEach(x => Console.WriteLine($"{x.Value}: {x.DisplayText}"));
                    break;
                case BotTransferMessage btm:
                    Console.WriteLine("bot requested to transfer the chat");
                    break;
            }
        }

    }
}
