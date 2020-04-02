using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageManager
{
    public class MessageServices : IMessageServices
    {
        private readonly IOptions<EmailConfiguration> config;

        public MessageServices(IOptions<EmailConfiguration> config)
        {
            this.config = config;
        }

        public void SendEmail(object Content)
        {
            string subject = config.Value.Subject;
            string to = config.Value.To;
            string from = config.Value.From;
            string smtpServer = config.Value.SmtpServer;
            int port = config.Value.Port;
            string password = config.Value.Password;
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Test Message", from));
            message.To.Add(new MailboxAddress("test", to));
            message.Subject = subject;

            string messageBody = string.Empty;
            var _content = Content as Exception;
            if (_content != null)
            {
                messageBody = _content.StackTrace;
            }
            else
            {
                messageBody = Content?.ToString();
            }
            message.Body = new TextPart("plain")
            {
                Text = messageBody
            };

            using (var emailclient = new SmtpClient())
            {
                emailclient.Connect(smtpServer, port, false);
                emailclient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailclient.Authenticate(from, password);
                emailclient.Send(message);
                emailclient.Disconnect(true);

            }

        }
    }
}
