using System.Threading.Tasks;

namespace CreenChat.WebAPI.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
