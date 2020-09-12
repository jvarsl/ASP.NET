using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SalonWithRazor.ServiceModels
{
    public class EmailService : IEmailSender
    {
        private readonly MessageSenderOptions _options;

        public EmailService(IOptions<MessageSenderOptions> options)
        {
            _options = options.Value;
        }
        
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential(_options.EmailUserName, _options.EmailPassword)
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("NO-REPLY@localhost.com")
            };
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = htmlMessage;
            mailMessage.IsBodyHtml = true;
            return client.SendMailAsync(mailMessage);
        }
    }
}
