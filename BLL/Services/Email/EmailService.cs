using BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Email
{
    public class EmailService:IEmailService
    {
        
            private readonly IConfiguration _config;

            public EmailService(IConfiguration config)
            {
                _config = config;
            }

            public async Task SendEmailAsync(string toEmail, string subject, string body)
            {
                var smtpClient = new SmtpClient(_config["EmailSettings:SmtpServer"])
                {
                    Port = int.Parse(_config["EmailSettings:Port"]),
                    Credentials = new NetworkCredential(
                        _config["EmailSettings:SenderEmail"],
                        _config["EmailSettings:SenderPassword"]
                    ),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_config["EmailSettings:SenderEmail"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
            }
        
    }
}
