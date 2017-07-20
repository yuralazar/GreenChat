using System;
using System.Collections.Generic;
using GreenChat.Data.Instances;
using GreenChat.Data.MessageTypes.SendArgs;

namespace GreenChat.Data.MessageTypes
{
    public static class SendTypes
    {
        public static Dictionary<SendActionTypes, SendMethodAndType> Dictionary = new Dictionary<SendActionTypes, SendMethodAndType>
        {
            { SendActionTypes.ErrorText,new SendMethodAndType("ErrorText", typeof(string)) },
            { SendActionTypes.ConnectionStatus, new SendMethodAndType("ConnectionStatus", typeof(UserInfo)) },
            { SendActionTypes.PrivateMessage,new SendMethodAndType("PrivateMessage", typeof(SendPrivateArguments)) },
            { SendActionTypes.ChatMessage,new SendMethodAndType("ChatMessage", typeof(SendChatArguments)) },
            { SendActionTypes.UserFound, new SendMethodAndType("UserFound", typeof(UserFoundArguments)) },
            { SendActionTypes.FriendRequest, new SendMethodAndType("FriendRequest",typeof(FriendRequestArguments)) },
            { SendActionTypes.FriendConfirmed, new SendMethodAndType("FriendConfirmed",typeof(FriendConfirmedArguments)) },
            { SendActionTypes.ChatCreated, new SendMethodAndType("ChatCreated",typeof(ChatCreatedArguments)) },
            { SendActionTypes.ChatConfirmed, new SendMethodAndType("ChatConfirmed",typeof(ChatConfirmedArguments)) },
            { SendActionTypes.ChatRequest, new SendMethodAndType("ChatRequest",typeof(ChatRequestArguments)) },
            { SendActionTypes.InitialInfo, new SendMethodAndType("InitialInfo",typeof(InitialInfo)) },
            { SendActionTypes.PrivateMessages, new SendMethodAndType("PrivateMessages",typeof(PrivateMessagesArguments)) },
            { SendActionTypes.ChatMessages, new SendMethodAndType("ChatMessages",typeof(ChatMessagesArguments)) }
        };

        public static string GetRecieveMethod(SendActionTypes type)
        {
            return Dictionary[type].MethodName;
        }

        public static Type GetRecieveArgumentsType(SendActionTypes type)
        {
            return Dictionary[type].Type;
        }
    }

    public enum SendActionTypes
    {
        ErrorText,                                      // 0
        ConnectionStatus,                               // 1
        PrivateMessage,                                 // 2
        ChatMessage,                                    // 3
        UserFound,                                      // 4
        FriendRequest,                                  // 5
        FriendConfirmed,                                // 6
        ChatCreated,                                    // 7
        ChatRequest,                                    // 8
        ChatConfirmed,                                  // 9        
        InitialInfo,                                    // 10         
        PrivateMessages,                                // 11
        ChatMessages,                                   // 12
        LeftChat,                                       // 13        
        PrivateMessageStatus,                           // 14
        ChatMessageStatus                               // 15
    }

    public struct SendMethodAndType
    {
        public string MethodName;
        public Type Type;

        public SendMethodAndType(string methodName, Type type)
        {
            MethodName = methodName;
            Type = type;
        }
    }

}
