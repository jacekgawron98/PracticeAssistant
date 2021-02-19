using Microsoft.Extensions.Options;
using PracticeAssistant.Config;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeAssistant.Services
{
    public class EmailService : IEmailService
    {
        public SendGridAuth Options { get; }
        public EmailService(IOptions<SendGridAuth> options)
        {
            Options = options.Value;
        }
        public Task SendEmailAsync(string email, string subject, string msg)
        {
            return Execute(Options.SendGridKey, email, subject, msg);
        }

        public Task Execute(string apiKey, string email, string subject, string msg)
        {
            var client = new SendGridClient(Options.SendGridKey);
            var message = new SendGridMessage()
            {
                From = new EmailAddress("gawrontest98@gmail.com", Options.SendGridUser),
                Subject = subject,
                PlainTextContent = msg,
                HtmlContent = msg
            };
            message.AddTo(new EmailAddress(email));
            message.SetClickTracking(false, false);
            return client.SendEmailAsync(message);
        }
    }
}
