using GreenChat.Data.Instances;

namespace GreenChat.Data.MessageTypes.SendArgs
{
    public class FriendRequestArguments : BaseSendArguments
    {
        public FriendRequestArguments()
        {
        }

        public FriendRequestArguments(string userId, string userEmail) : base(new UserInfo(userId, userEmail))
        {
        }
    }
}