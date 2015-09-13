#Twitter Bot

Very simple ASP.Net MVC 4 application (one controller, one action, one view) that can post to twitter.

Specifically it looks up google calendar's public christian holiday calendar, gets all events from the start of the current year for the next two years and uses the values for Ash Wednesday and Easter Sunday to determine when Lent starts or if in Lent when it will end.

Initially inspired by this:
http://www.billthelizard.com/2013/12/creating-twitter-bot-on-google-app.html

Used this to generate the basic ASP.Net MVC 4 application:
http://mvc4beginner.com/Tutorial/MVC-Hello-World-Barebone.html

Get Google Calendar packages:
https://www.nuget.org/packages/Google.Apis.Calendar.v3/

Newtownsoft version issues:
http://stackoverflow.com/questions/6176841/could-not-load-file-or-assembly-newtonsoft-json-version-3-5-0-0/24206229#24206229

Google Calendar with service account issues:
http://stackoverflow.com/questions/26517737/trying-using-oauth2-causes-a-not-authorized-client-or-scope-error/26548375#26548375

Get GMail packages:
https://www.nuget.org/packages/Google.Apis.Gmail.v1/

Warning
1>  Consider app.config remapping of assembly "Newtonsoft.Json, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed" from Version "4.5.0.0" [C:\Users\498084\Documents\GitHub\Twitter-Bot\TwitterBot\packages\TweetSharp.2.3.1\lib\4.0\Newtonsoft.Json.dll] to Version "6.0.0.0" [C:\Users\498084\Documents\GitHub\Twitter-Bot\TwitterBot\packages\Newtonsoft.Json.6.0.6\lib\net40\Newtonsoft.Json.dll] to solve conflict and get rid of warning.
1>C:\Windows\Microsoft.NET\Framework\v4.0.30319\Microsoft.Common.targets(1605,5): warning MSB3247: Found conflicts between different versions of the same dependent assembly.

https://social.msdn.microsoft.com/Forums/vstudio/en-US/faa1b607-50bb-48e3-bd5d-76f4fc02ad4c/ms-build-gives-warning-msb3247-found-conflicts-between-different-versions-of-same-dependent?forum=msbuild

Links: