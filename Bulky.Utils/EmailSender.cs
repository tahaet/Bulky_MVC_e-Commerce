using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Mail;

namespace BulkyBook.Utility
{
    public class EmailSender:IEmailSender
    {
        private readonly IConfiguration config;

        public EmailSender(IConfiguration config)
        {
            this.config = config;
            
        }
        public async Task SendEmailAsync(string email,string subject,string htmlMessage)
        {
            var fromMail = config.GetValue<string>("MailService:fromMail");
            var fromPassword = config.GetValue<string>("MailService:fromPassword");
            var message = new MailMessage
            {
                From = new MailAddress(fromMail),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            message.To.Add(email);
            using (var smptClient = new SmtpClient("smtp.mail.yahoo.com", 587)
            {
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
            })
            {

                try
                {
                    await smptClient.SendMailAsync(message);
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine($"SmtpException: {ex.Message}");
                    Console.WriteLine($"StatusCode: {ex.StatusCode}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                    throw;
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"IOException: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected Exception: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                    throw;
                }
            } 
        }
    }
}
