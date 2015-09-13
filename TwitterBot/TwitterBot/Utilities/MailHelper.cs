using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace TwitterBot.Utilities
{
    public class MailHelper
    {
        private readonly string _mSenderAddress;
        private readonly string _mReceiverAddress;

        public MailHelper(string senderAddress,
                          string receiverAddress)
        {
            _mSenderAddress = senderAddress;
            _mReceiverAddress = receiverAddress;
        }

        public void Send(string subject,
                         string body)
        {
            SendEmail(_mSenderAddress, _mReceiverAddress, subject, body);
        }

        public static void SendEmail(string senderAddress,
            string receiverAddress,
            string subject,
            string body)
        {
            try
            {
                var smtpClient = new SmtpClient();

                smtpClient.Send(new MailMessage(senderAddress, receiverAddress, subject, body));
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Exception in MailHelper SendEmail: {0}", e.Message));
            }
        }
    }
}