using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using TwitterBot.Utilities;

namespace TwitterBot.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var sendEmailAddress = ConfigurationManager.AppSettings["MailSenderAddress"];
            var receiveEmailAddress = ConfigurationManager.AppSettings["MailReceiverAddress"];
            const string subject = @"LentBot error!";

            var mailHelper = new MailHelper(sendEmailAddress, receiveEmailAddress);

            // get datetime for now, resetting time to midnight
            // only deal with midnight datetimes so differences can be done in whole days
            var now = DateTime.Now.Date;

            // 2015 test dates
            // New year
            // now = new DateTime(2015, 1, 1);
            // Month before
            // now = new DateTime(2015, 1, 18);
            // Week before
            // now = new DateTime(2015, 2, 11);
            // Shrove Tuesday
            // now = new DateTime(2015, 2, 17);

            // Ash Wednesday
            // now = new DateTime(2015, 2, 18);
            // One month
            // now = new DateTime(2015, 3, 5);
            // One week
            // now = new DateTime(2015, 3, 29);
            // Weeks
            // now = new DateTime(2015, 3, 15);
            // Good Friday
            // now = new DateTime(2015, 4, 3);
            // Holy Saturday
            // now = new DateTime(2015, 4, 4);
            // Easter Sunday
            // now = new DateTime(2015, 4, 5);
            // Otherwise however many days
            // now = new DateTime(2015, 3, 17);

            // Easter Monday
            // now = new DateTime(2015, 4, 6);
            // Christmas
            // now = new DateTime(2015, 12, 25);

            ViewBag.Now = now.ToString("MMMM dd yyyy");

            var tweetMessage = string.Empty;

            try
            {
                var startQuery = new DateTime(now.Year - 1, 12, 31, 23, 59, 59);
                var endQuery = startQuery.AddYears(2);

                var google = new GoogleApiHelper(
                    string.Format("{0}bin\\{1}", Server.MapPath("~"), ConfigurationManager.AppSettings["GoogleP12FileName"]),
                    ConfigurationManager.AppSettings["GoogleServiceAccountNameUserEmail"],
                    ConfigurationManager.AppSettings["GoogleApplicationName"],
                    ConfigurationManager.AppSettings["GoogleCalendarId"],
                    startQuery,
                    endQuery);

                var ashWednesdayList = google.GetEventsBySummary("ash");

                if (ashWednesdayList.Count() != 2)
                {
                    var body =
                        string.Format(
                            "Incorrect number of Ash Wednesdays found in calendar query, count: {0} start: {1} end: {2}",
                            ashWednesdayList.Count, startQuery, endQuery);

                    mailHelper.Send(subject,
                                    body);

                    ViewBag.Error += " " + body;
                }

                var easterSundayList = google.GetEventsBySummary("Easter Sunday");

                if (easterSundayList.Count() != 2)
                {
                    var body =
                        string.Format(
                            "Incorrect number of Easter Sundays found in calendar query, count: {0} start: {1} end: {2}",
                            easterSundayList.Count, startQuery, endQuery);

                    mailHelper.Send(subject,
                                    body);

                    ViewBag.Error += " " + body;
                }

                var ashWednesday = ashWednesdayList[0].Date;
                var easterSunday = easterSundayList[0].Date;

                var lent = easterSunday - ashWednesday;

                if (lent.Days != 46)
                {
                    var body = string.Format("Incorrect number of days in lent, days: {0} start: {1} end: {2}",
                        lent.Days, startQuery, endQuery);

                    mailHelper.Send(subject,
                                    body);
                }

                // Before lent
                if (now < ashWednesday)
                {
                    var wait = ashWednesday - now;

                    var days = wait.Days;

                    // New year
                    if ((now.Month == 1) &&
                        (now.Day == 1))
                    {
                        tweetMessage = string.Format("Happy New Year!  Its {0} days until Ash Wednesday ({1}).", days, ashWednesday.ToString("MMMM dd, yyyy"));
                    }
                    // Month before
                    else if ((now.Month == (ashWednesday.Month - 1)) && 
                        (now.Day == ashWednesday.Day))
                    {
                        tweetMessage = string.Format("Just one month until Ash Wednesday ({0}).", ashWednesday.ToString("MMMM dd, yyyy"));
                    }
                    // Week before
                    else switch (days)
                    {
                        case 7:
                            tweetMessage = string.Format("Only one week until Ash Wednesday ({0}).", ashWednesday.ToString("MMMM dd, yyyy"));
                            break;
                        case 1:
                            tweetMessage = "Its Shrove Tuesday, Lent starts tomorrow so get those pancakes eaten!";
                            break;
                    }

                    ViewBag.WaitMessage = string.Format("Its {0} days until Ash Wednesday", days);
                }
                // In lent
                else if ((now >= ashWednesday) && (now <= easterSunday))
                {
                    var wait = easterSunday - now;

                    var days = wait.Days;

                    var dayCount = string.Format("today is day {0}/{1} of Lent", (lent.Days - days), lent.Days);

                    // Special
                    // Ash Wednesday
                    if ((now.Month == ashWednesday.Month) &&
                        (now.Day == ashWednesday.Day))
                    {
                        tweetMessage = string.Format("Its Ash Wednesday, {0} days of Lent ahead until Easter Sunday ({1}).", days, easterSunday.ToString("MMMM dd, yyyy"));
                    }
                    // One month
                    else if ((now.Month == (easterSunday.Month - 1)) &&
                        (now.Day == easterSunday.Day))
                    {
                        tweetMessage = string.Format("One month until Easter Sunday ({0}), {1}.", easterSunday.ToString("MMMM dd, yyyy"), dayCount);
                    }
                    // One week
                    else if (days == 7)
                    {
                        tweetMessage = string.Format("One week until Easter Sunday ({0}), {1}.", easterSunday.ToString("MMMM dd, yyyy"), dayCount);
                    }
                    // Weeks
                    else if ((days > 0) &&
                        ((days % 7) == 0))
                    {
                        tweetMessage = string.Format("{0} weeks until Easter Sunday ({1}), {2}.", (days/7), easterSunday.ToString("MMMM dd, yyyy"), dayCount);
                    }
                    // Good Friday
                    else if (days == 2)
                    {
                        tweetMessage = string.Format("Its Good Friday, {0}.", dayCount);
                    }
                    // Holy Saturday
                    else if (days == 1)
                    {
                        tweetMessage = string.Format("Its Holy Saturday, {0}.", dayCount);
                    }
                    // Easter Sunday
                    else if (days == 0)
                    {
                        tweetMessage = string.Format("Happy Easter, today is day {0}/{1} and Lent is over, well done!", (lent.Days - days), lent.Days);
                    }
                    // Otherwise however many days
                    else
                    {
                        tweetMessage = string.Format("Its day {0}/{1} of Lent.", (lent.Days - days), lent.Days);
                    }

                    ViewBag.WaitMessage = string.Format("Its day {0}/{1} of Lent.", (lent.Days - days), lent.Days);
                }
                // Next lent
                else
                {
                    var lastEasterSunday = easterSunday;

                    ashWednesday = ashWednesdayList[1].Date;
                    easterSunday = easterSundayList[1].Date;

                    // this is wasteful, validation is duplicated from above
                    lent = easterSunday - ashWednesday;

                    if (lent.Days != 46)
                    {
                        var body = string.Format("Incorrect number of days in lent, days: {0} start: {1} end: {2}",
                            lent.Days, startQuery, endQuery);

                        mailHelper.Send(subject,
                                        body);
                    }

                    var wait = ashWednesday - now;

                    var days = wait.Days;

                    var passed = lastEasterSunday - now;

                    // day after
                    if (passed.Days == -1)
                    {
                        tweetMessage = string.Format("Its Easter Monday, next year Ash Wednesday is {0} and Easter Sunday is {1}.", ashWednesday.ToString("MMMM dd, yyyy"), easterSunday.ToString("MMMM dd, yyyy"));
                    }
                    // Christmas
                    else if ((now.Month == 12) &&
                        (now.Day == 25))
                    {
                        tweetMessage = string.Format("Merry Christmas!  Its {0} days until Ash Wednesday ({1}).", days, ashWednesday.ToString("MMMM dd, yyyy"));
                    }

                    ViewBag.WaitMessage = string.Format("Its {0} days until Ash Wednesday next year", days);
                }

                ViewBag.NextAshWednesday = ashWednesday.ToString("MMMM dd yyyy");
                ViewBag.NextEaster = easterSunday.ToString("MMMM dd yyyy");
                ViewBag.LentLength = lent.Days;

                if (!string.IsNullOrEmpty(tweetMessage))
                {
                    // add hashtags
                    tweetMessage = string.Format("{0} #Lent #Easter #Lent{1} #Easter{1}", tweetMessage, easterSunday.Year);

                    ViewBag.TweetMessage = tweetMessage;

                    // tweet check length <= 140
                    if (tweetMessage.Length > 140)
                    {
                        var body = string.Format("Tweet was too big {0}: '{1}'", tweetMessage.Length, tweetMessage);

                        mailHelper.Send(subject,
                                        body);

                        tweetMessage = string.Empty;

                        ViewBag.Error += " " + body;
                    }
                }
            }
            catch (Exception e)
            {
                var body = string.Format("\n\nError in the Google Calendar bit: {0}", e.Message);

                mailHelper.Send(subject,
                                body);

                ViewBag.Error += " " + body;
            }

            try
            {
                if (!string.IsNullOrEmpty(tweetMessage))
                {
                    var tweet = new TwitterApiHelper(
                        ConfigurationManager.AppSettings["TwitterConsumerKey"],
                        ConfigurationManager.AppSettings["TwitterConsumerSecret"],
                        ConfigurationManager.AppSettings["TwitterToken"],
                        ConfigurationManager.AppSettings["TwitterTokenSecret"]);

                    ViewBag.HasTweeted = tweet.HasTweetedToday;

                    if (!tweet.HasTweetedToday)
                    {
                        var sent = tweet.SendTweet(tweetMessage);

                        ViewBag.TweetSuccessful = sent;
                    }
                    else
                    {
                        ViewBag.AlreadyTweetedTime = tweet.TimeOfLastTweet;
                    }
                }
            }
            catch (Exception e)
            {
                var body = string.Format("\n\nError in the Twitter bit: {0}", e.Message);

                mailHelper.Send(subject,
                                body);
            
                ViewBag.error += " " + body;
            }

            return View();
        }

    }
}
