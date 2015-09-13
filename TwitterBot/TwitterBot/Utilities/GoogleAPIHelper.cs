using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;


namespace TwitterBot.Utilities
{
    public class GoogleApiHelper
    {
        private readonly List<GoogleCalendarEvent> _mEventList;

        public GoogleApiHelper(
            string p12Path,
            string serviceAccountEmail,
            string applicationName, 
            string calendarId,
            DateTime startCalendarQuery,
            DateTime endCalendarQuery)
        {
            try
            {
                var certificate = new X509Certificate2(p12Path, "notasecret", X509KeyStorageFlags.Exportable);

                var credential = new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(serviceAccountEmail)
                    {
                        Scopes = new[] { CalendarService.Scope.Calendar }
                    }.FromCertificate
                    (certificate));

                var initializer = new BaseClientService.Initializer();
                initializer.HttpClientInitializer = credential;
                initializer.ApplicationName = applicationName;

                var service = new CalendarService(initializer);

                var queryStart = startCalendarQuery;
                var queryEnd = endCalendarQuery;

                var query = service.Events.List(calendarId);
                query.TimeMin = queryStart;
                query.TimeMax = queryEnd;

                var events = query.Execute().Items;

                _mEventList = events.Select(e => new GoogleCalendarEvent(DateTime.Parse(e.Start.Date), e.Summary)).ToList();

                _mEventList.Sort((e1, e2) => e1.Date.CompareTo(e2.Date));
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Exception in GoogleAPIHelper constructor: {0}", e.Message));
            }
        }

        public int EventCount
        {
            get
            {
                var result = 0;

                if (this._mEventList != null)
                {
                    result = this._mEventList.Count;
                }

                return result;
            }
        }

        public IList<GoogleCalendarEvent> GetEventsBySummary(string summaryFilter)
        {
            var result = new List<GoogleCalendarEvent>();

            if (this._mEventList != null)
            {
                result = this._mEventList.Where(e => e.Summary.IndexOf(summaryFilter, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            return result;
        }
    }
}