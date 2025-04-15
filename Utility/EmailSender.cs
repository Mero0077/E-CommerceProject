using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace E_CommerceFIdentityScaff.Utility
{
    public class EmailSender:IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("your.email@live.com", "your password")
            };

            return client.SendMailAsync(
                new MailMessage(from: "your.email@live.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
