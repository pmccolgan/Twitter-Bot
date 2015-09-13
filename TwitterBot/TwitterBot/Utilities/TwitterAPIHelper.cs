using System;
using TweetSharp;

namespace TwitterBot.Utilities
{
    public class TwitterApiHelper
    {
        private readonly TwitterService m_service;

        public TwitterApiHelper(
            string consumerKey,
            string consumerSecret,
            string token,
            string tokenSecret)
        {
            try
            {
                this.m_service = new TwitterService(consumerKey, consumerSecret);
                this.m_service.AuthenticateWith(token, tokenSecret);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Exception in TwitterAPIHelper constructor: {0}", e.Message));
            }
        }

        public bool SendTweet(string body)
        {
            var result = false;

            try
            {
                var options = new SendTweetOptions
                {
                    Status = body
                };

                var status = this.m_service.SendTweet(options);

                result = status.Id > 0;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Exception in TwitterAPIHelper SendTweet: {0}", e.Message));
            }

            return result;
        }

        public DateTime TimeOfLastTweet
        {
            get
            {
                var result = new DateTime();

                try
                {
                    var options = new GetUserProfileOptions();

                    var user = this.m_service.GetUserProfile(options);

                    if ((user != null) &&
                        (user.Status != null))
                    {
                        result = user.Status.CreatedDate;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("Exception in TwitterAPIHelper TimeOfLastTweet: {0}", e.Message));
                }

                return result;
            }
        }

        public bool HasTweetedToday
        {
            get
            {
                var result = false;

                try
                {
                    var lastTweet = this.TimeOfLastTweet;
                    
                    if ((lastTweet == new DateTime()) ||
                        (lastTweet >= DateTime.Today))
                    {
                        // set to true if time of last tweet came through as default, so it doesn't spam if it encounters an issue
                        result = true;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("Exception in TwitterAPIHelper HasTweetedToday: {0}", e.Message));
                }

                return result;
            }
        }
    }
}