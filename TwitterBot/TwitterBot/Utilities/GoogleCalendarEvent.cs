using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterBot.Utilities
{
    public class GoogleCalendarEvent
    {
        public GoogleCalendarEvent(DateTime date, string summary)
        {
            this.Date = date;
            this.Summary = summary;
        }

        public DateTime Date { get; private set; }

        public string Summary { get; private set; }
    }
}