using System.Threading.Tasks;

namespace GreenChat.WebAPI.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
