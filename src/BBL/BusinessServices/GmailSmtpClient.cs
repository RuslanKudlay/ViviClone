using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Application.BBL.BusinessServices
{
    public class GmailSmtpClient : ISmtpClient
    {
        public string SmtpServerName { get; } = "smtp.gmail.com";

        private readonly string from = "";
        private readonly string password = "";
        private readonly ILogger<GmailSmtpClient> _logger;

        public GmailSmtpClient(IConfiguration configuration, ILogger<GmailSmtpClient> logger)
        {
            from = configuration.GetSection("Data:GmailSmtpClient:Email").Value;
            password = configuration.GetSection("Data:GmailSmtpClient:Password").Value;
            _logger = logger;
        }

        public async Task SendMail(string to, string subject, string body)
        {
            string smt = "";
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(from);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(SmtpServerName, 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(from, password);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail).ContinueWith((res) =>
                    {
                        if (res.Exception?.Message != null && res.Exception?.Message?.Length > 0)
                        {
                            _logger.Log(LogLevel.Information, new EventId(LoggerId.Information),
                                "An error occured while sending email");
                        }
                    });
                }
            }
        }
    }
}
