using System;
using System.Collections.Generic;
using GreenChat.Data.MessageTypes.RecieveArgs;

namespace GreenChat.Data.MessageTypes
{
    public static class RecieveTypes
    {
        public static Dictionary<int, RecieveMethodAndType> Dictionary = new Dictionary<int, RecieveMethodAndType>
        {
            { 0, new RecieveMethodAndType("RecievePrivate", typeof(RecievePrivateArguments))},
            { 1, new RecieveMethodAndType("RecieveChat", typeof(RecieveChatArguments)) },            
            { 2, new RecieveMethodAndType("SearchFriend", typeof(SearchFriendArguments)) },            
            { 3, new RecieveMethodAndType("AddFriend", typeof(AddFriendArguments)) },            
            { 4, new RecieveMethodAndType("CreateChat", typeof(CreateChatArguments)) },
            { 5, new RecieveMethodAndType("InviteToChat", typeof(InviteToChatArguments)) },
            { 6, new RecieveMethodAndType("ConfirmChat", typeof(ConfirmChatArguments)) },
            { 7, new RecieveMethodAndType("ReadPrivateMessages", typeof(ReadPrivateMessagesArguments)) },
            { 8, new RecieveMethodAndType("ReadChatMessages", typeof(ReadChatMessagesArguments)) },
            { 9, new RecieveMethodAndType("GetPrivateMessages", typeof(GetPrivateMessagesArguments)) },
            { 10, new RecieveMethodAndType("GetChatMessages", typeof(GetChatMessagesArguments)) },
            { 11, new RecieveMethodAndType("PrivateMessageStatus", typeof(PrivateStatusArguments)) },
            { 12, new RecieveMethodAndType("ChatMessageStatus", typeof(ChatStatusArguments)) }
        };

        public static string GetRecieveMethod(int typeNumber)
        {
            return Dictionary[typeNumber].MethodName;
        }

        public static Type GetRecieveArgumentsType(int typeNumber)
        {
            return Dictionary[typeNumber].Type;
        }

        public static int GetActionType(Type type)
        {            
            foreach (var row in Dictionary)
            {
                if (row.Value.Type == type)
                    return row.Key;
            }

            throw new ArgumentException("Wrong type of Recieve arguments was given!");
        }
    }

    public struct RecieveMethodAndType
    {
        public string MethodName;
        public Type Type;

        public RecieveMethodAndType(string methodName, Type type)
        {
            MethodName = methodName;
            Type = type;
        }
    }

}
