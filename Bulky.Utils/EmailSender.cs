using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace BulkyBook.Utility
{
    public class EmailSender:IEmailSender
    {
        private readonly IConfiguration config;

        public string MailServiceSecretKey { get; set; }
        public EmailSender(IConfiguration config)
        {
            this.config = config;
            MailServiceSecretKey = this.config.GetValue<string>("MailService:SecretKey");
        }
        public Task SendEmailAsync(string email,string subject,string htmlMessage)
        {
            //logic to send emails via any mail service
            return Task.CompletedTask;
        }
    }
}
