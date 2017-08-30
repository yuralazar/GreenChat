using System.Collections.Generic;
using System.Threading.Tasks;
using GreenChat.DAL.Models;

namespace GreenChat.DAL.Interfaces
{
    public interface IPrivateMessageStatusesRepository : IMessageStatusesRepository<PrivateMessageStatus>
    {        
    }
}