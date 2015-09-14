#Twitter Bot

Very simple ASP.Net MVC 4 application (one controller, one action, one view) that can post to twitter.

Specifically when run the application looks up google calendar's public christian holiday calendar, gets all events from the start of the current year for the next two years and uses the values for Ash Wednesday and Easter Sunday to determine when Lent starts or if in Lent when it will end.  Then it decides based on this whether or not it should tweet about it.  To ensure the app is run daily a separate application runs on a background worker, the source for this application is available at [Twitter Bot Ping] (https://github.com/pmccolgan/Twitter-Bot-Ping).  Any errors and it should email a report out.

Initially inspired by this post: http://www.billthelizard.com/2013/12/creating-twitter-bot-on-google-app.html

Used this to generate the basic ASP.Net MVC 4 application: http://mvc4beginner.com/Tutorial/MVC-Hello-World-Barebone.html

[Google Calendar API] (https://github.com/pmccolgan/Google-Calendar-API-v3-Simple-Console-Demo) and [Twitter API] (https://github.com/pmccolgan/Twitter-API-Simple-Console-Demo) based on previous work.

Ended up not using the [GMail API] (https://github.com/pmccolgan/Gmail-API-Simple-Console-Demo) as [you can't access it with a service account] (http://stackoverflow.com/questions/26517737/trying-using-oauth2-causes-a-not-authorized-client-or-scope-error/26548375#26548375), just used the Mailgun add-on which was very straight-forward.

Google, Twitter and mail all need configured in the Web.config.

```
    <add key="GoogleP12FileName" value="YOU_SHOULD_COMPLETE_THIS" />
    <add key="GoogleServiceAccountNameUserEmail" value="YOU_SHOULD_COMPLETE_THIS" />
    <add key="GoogleUserEmail" value="YOU_SHOULD_COMPLETE_THIS" />
    <add key="GoogleApplicationName" value="YOU_SHOULD_COMPLETE_THIS" />
    <add key="GoogleCalendarId" value="YOU_SHOULD_COMPLETE_THIS" />
    <add key="MailHost" value="YOU_SHOULD_COMPLETE_THIS" />
    <add key="MailPort" value="YOU_SHOULD_COMPLETE_THIS" />
    <add key="MailUserName" value="YOU_SHOULD_COMPLETE_THIS" />
    <add key="MailPassword" value="YOU_SHOULD_COMPLETE_THIS" />
    <add key="MailSenderAddress" value="YOU_SHOULD_COMPLETE_THIS" />
    <add key="MailReceiverAddress" value="YOU_SHOULD_COMPLETE_THIS" />
    <add key="TwitterConsumerKey" value="YOU_SHOULD_COMPLETE_THIS" />
    <add key="TwitterConsumerSecret" value="YOU_SHOULD_COMPLETE_THIS" />
    <add key="TwitterToken" value="YOU_SHOULD_COMPLETE_THIS" />
    <add key="TwitterTokenSecret" value="YOU_SHOULD_COMPLETE_THIS" />
```
The final application is hosted on [AppHarbor] (https://appharbor.com/) and available [here] (http://lentbot.apphb.com/).