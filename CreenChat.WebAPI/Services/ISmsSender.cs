using System.Threading.Tasks;

namespace CreenChat.WebAPI.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
