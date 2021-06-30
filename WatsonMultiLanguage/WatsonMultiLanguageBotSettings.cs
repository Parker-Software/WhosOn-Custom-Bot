using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhosOnBot
{
    public class WatsonMultiLanguageBotSettings
    {
        public List<WatsonMultiLanguageBotLanguageSetting> langauges { get; set; }
    }

    public class WatsonMultiLanguageBotLanguageSetting
    {
        public string langCode { get; set; }
        public WatsonMultiLanguageBotLanguageServiceCredentials serviceCredentials { get; set; }
        public WatsonMultiLanguageBotLanguageIAM iam { get; set; }
    }

    public class WatsonMultiLanguageBotLanguageIAM
    {
        public string apiKey { get; set; }
        public string endpoint { get; set; }
        public string assistantId { get; set; }
    }

    public class WatsonMultiLanguageBotLanguageServiceCredentials
    {
        public string username { get; set; }
        public string password { get; set; }
        public string endpoint { get; set; }
        public string assistantId { get; set; }
    }
}
