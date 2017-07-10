function sendMessageAction() {
    var messageInput = document.getElementById("messageInput");
    var messageText = messageInput.value;

    if (messageText === "") return;

    var message = {
        text: messageText,
        date: new Date
    }

    if (currentMode === 0) {
        if (currentFriend === null)
            return;
        sendPrivateAction(currentFriend, message);
        createAndAppendPrivateMess(currentFriend, message, false);
    }
    else {
        if (currentChat === null)
            return;
        sendChatAction(currentChat, message);
        createAndAppendChatMess(currentUser, currentChat, message, false);
    }
    messageInput.value = "";

}

function sendPrivateAction(user, message) {
    var arguments =
        {
            "socketId": connection.socketId,
            "userFrom": currentUser,
            "message": message,
            "userTo":
            {
                "id": user.id,
                "email": user.email
            }
        };
    var obj =
        {
            "recieveType": 0,
            "arguments": JSON.stringify(arguments)
        }
    // RecievePrivate - server side method to be invoked
    connection.invoke(obj);
}

function sendChatAction(chat, message) {
    var arguments =
        {
            "socketId": connection.socketId,
            "userFrom": currentUser,
            "message": message,
            "chat":
            {
                "id": chat.id,
                "name": chat.name
            }
        };
    var obj =
        {
            "recieveType": 1,
            "arguments": JSON.stringify(arguments)
        }
    // RecieveChat - server side method to be invoked
    connection.invoke(obj);
}

function searchFriendAction() {
    var searchEmail = document.getElementById("searchEmail");
    if (searchEmail.value == "") return;

    var arguments =
        {
            "socketId": connection.socketId,
            "userFrom": currentUser,
            "searchEmail": searchEmail.value
        };
    var obj =
        {
            "recieveType": 2,
            "arguments": JSON.stringify(arguments)
        }
    //AddFriend - server side method to be invoked
    connection.invoke(obj);
    searchEmail.value = "";
}

function addOrConfirmFriend(initiator, confirmed, user) {
    if (user === null)
        return;
    var arguments =
        {
            "socketId": connection.socketId,
            "userFrom": currentUser,
            "initiator": initiator,
            "confirmed": confirmed,
            "user":
            {
                "id": user.id,
                "email": user.email
            }
        };
    var obj =
        {
            "recieveType": 3,
            "arguments": JSON.stringify(arguments)
        }
    //SearchFriend - server side method to be invoked
    connection.invoke(obj);
};

function createChatAction() {
    var chatName = document.getElementById("chatName");
    var arguments =
        {
            "socketId": connection.socketId,
            "userFrom": currentUser,
            "chatName": chatName.value
        };
    var obj =
        {
            "recieveType": 5,
            "arguments": JSON.stringify(arguments)
        }
    // CreateChat - server side method to be invoked
    connection.invoke(obj);
    chatName.value = "";
}

function addToChat(chat) {
    var chatOptions = document.getElementById("selectUserToChat").options;
    var selectedUserIndex = chatOptions.selectedIndex;
    var selectedUser = chatOptions[selectedUserIndex];
    var arguments =
        {
            "socketId": connection.socketId,
            "userFrom": currentUser,
            "chat":
            {
                "id": chat.id,
                "name": chat.name
            },
            "users":
            [
                {
                    "id": selectedUser.id,
                    "email": selectedUser.text
                }
            ]
        };
    var obj =
        {
            "recieveType": 6,
            "arguments": JSON.stringify(arguments)
        }
    // InviteToChat - server side method to be invoked
    connection.invoke(obj);
    appendInfoMessage("Friend " + selectedUser.text + " was added to chat '" + chat.name + "'");
}

function confirmChatRequest(chat, confirmed) {
    var arguments =
        {
            "socketId": connection.socketId,
            "userFrom": currentUser,
            "confirmed": confirmed,
            "chat":
            {
                "id": chat.id,
                "name": chat.name
            }
        };
    var obj =
        {
            "recieveType": 7,
            "arguments": JSON.stringify(arguments)
        }
    // ConfirmChat - server side method to be invoked
    connection.invoke(obj);
}