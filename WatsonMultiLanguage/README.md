A custom WhosOn bot designed to incorporate multiple langauge chat bots using a single bot. The bot is built on top of the existing Watson bot and can be configured using the "Custom bot options" field in the WhosOn settings portal.

The bot looks for a property called **langauge** when the conversation is created, and forwards the requests onto the relevant bot based on that value.

The configuration JSON should be in the following format:

```
{
    "en_apikey":"",
    "en_endpoint":"",
    "en_assistantId":"",
    "fr_apikey":"",
    "fr_endpoint":"",
    "fr_assistantId":""
}
```
Credentials for Watson can be provided as both IAM and Service Credentials e.g.

```
{
    "en_username":"",
    "en_password":"",
    "en_endpoint":"",
    "en_assistantId":"",
    "fr_apikey":"",
    "fr_endpoint":"",
    "fr_assistantId":""
}
```

