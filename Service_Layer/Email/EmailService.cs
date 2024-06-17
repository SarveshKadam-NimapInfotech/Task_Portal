using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Task_Portal.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async System.Threading.Tasks.Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient
                {
                    Host = _configuration["SmtpSettings:Server"],
                    Port = int.Parse(_configuration["SmtpSettings:Port"]),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_configuration["SmtpSettings:SenderEmail"], _configuration["SmtpSettings:Password"])
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["SmtpSettings:SenderEmail"], _configuration["SmtpSettings:MailFromDisplayName"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(to);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                throw new InvalidOperationException("Could not send email", ex);
            }

        }
    }
}
