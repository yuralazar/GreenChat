using System.Threading.Tasks;

namespace GreenChat.Client_WEB.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
