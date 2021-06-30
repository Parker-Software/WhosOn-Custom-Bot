using Parkersoft.WhosOn.BotConnector;
using System;
using System.Linq;
using System.Text;

namespace WhosOnBot
{

    public class WatsonMultiLanguageBotCreator : IBotCreator
    {
        public Delegates.CreateBot DoCreateBot
        {
            get
            {
                return ((settings) =>
                {
                    WatsonMultiLanguageBotSettings botSettings = new WatsonMultiLanguageBotSettings
                    {
                        langauges = new System.Collections.Generic.List<WatsonMultiLanguageBotLanguageSetting>()
                    };

                    // Get languages
                    string[] languages = settings.Properties.Keys.Where(a => a.Contains("_")).Select(a => a.Split('_')[0]).ToArray();

                    // Get language bot details
                    foreach(string lang in languages)
                    {
                        var langValues = settings.Properties.Where(a => a.Key.StartsWith($"{lang}_")).ToList();
                        if(langValues.Any(a => a.Key == $"{lang}_apikey"))
                        {
                            // IAM
                            botSettings.langauges.Add(new WatsonMultiLanguageBotLanguageSetting
                            {
                                langCode = lang,
                                iam = new WatsonMultiLanguageBotLanguageIAM
                                {
                                    apiKey = langValues.Where(a => a.Key.Contains("apikey")).Select(a => a.Value).FirstOrDefault(),
                                    endpoint = langValues.Where(a => a.Key.Contains("endpoint")).Select(a => a.Value).FirstOrDefault(),
                                    assistantId = langValues.Where(a => a.Key.Contains("assistantId")).Select(a => a.Value).FirstOrDefault(),
                                }
                            });
                        }
                        else
                        {
                            // Service
                            botSettings.langauges.Add(new WatsonMultiLanguageBotLanguageSetting
                            {
                                langCode = lang,
                                serviceCredentials = new WatsonMultiLanguageBotLanguageServiceCredentials
                                {
                                    username = langValues.Where(a => a.Key.Contains("username")).Select(a => a.Value).FirstOrDefault(),
                                    password = langValues.Where(a => a.Key.Contains("password")).Select(a => a.Value).FirstOrDefault(),
                                    endpoint = langValues.Where(a => a.Key.Contains("endpoint")).Select(a => a.Value).FirstOrDefault(),
                                    assistantId = langValues.Where(a => a.Key.Contains("assistantId")).Select(a => a.Value).FirstOrDefault(),
                                }
                            });
                        }
                    }

                    // Create bot
                    return new WatsonMultiLanguageBot(botSettings);
                });
            }
        }

        public string BotType => "WatsonMultiLanguageBot";
    }
}
