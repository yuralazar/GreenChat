using System.Threading.Tasks;

namespace GreenChat.WebAPI.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
