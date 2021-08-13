using System.Threading.Tasks;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface ISmtpClient
    {
        string SmtpServerName { get; }

        Task SendMail(string to, string subject, string body);
    }
}
