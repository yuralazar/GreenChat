using System.Threading.Tasks;

namespace GreenChat.Client_WEB.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
