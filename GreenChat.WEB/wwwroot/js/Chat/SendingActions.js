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
        sendPrivate(currentFriend, message);
        createAndAppendPrivateMess(currentFriend, message, false);
    }
    else {
        if (currentChat === null)
            return;
        sendChat(currentChat, message);
        createAndAppendChatMess(currentUser, currentChat, message, false);
    }
    messageInput.value = "";
    
}

function searchFriendAction() {
    var searchEmail = document.getElementById("searchEmail");
    if (searchEmail.value === "") return;

    searchFriend(searchEmail.value);
    searchEmail.value = "";
}

function createChatAction() {
    var chatName = document.getElementById("chatName");
    createChat(chatName.value);
    chatName.value = "";
}

function addToChatAction(chat) {
    var chatOptions = document.getElementById("selectUserToChat").options;
    var selectedUserIndex = chatOptions.selectedIndex;
    var selectedUser = chatOptions[selectedUserIndex];

    var users = [];
    users.push({ "id": selectedUser.id, "email": selectedUser.value});

    addToChat(chat, users);

    appendInfoMessage("Friend " + selectedUser.text + " was added to chat '" + chat.name + "'");
}

function sendPrivate(user, message) {
    var arguments =
        {       
            "userFrom": currentUser,
            "message": message,
            "userTo": user
        };
    var obj =
        {
            "type": 0,
            "arguments": JSON.stringify(arguments)
        }
    // RecievePrivate - server side method to be invoked
    connection.invoke(obj);
}

function sendChat(chat, message) {
    var arguments =
        {                     
            "userFrom": currentUser,
            "message": message,
            "chat": chat
        };
    var obj =
        {
            "type": 1,
            "arguments": JSON.stringify(arguments)
        }
    // RecieveChat - server side method to be invoked
    connection.invoke(obj);   
}

function searchFriend(email) {
    var arguments =
        {
            "userFrom": currentUser,
            "searchEmail": email
        };
    var obj =
        {
            "type": 2,
            "arguments": JSON.stringify(arguments)
        }
    //AddFriend - server side method to be invoked
    connection.invoke(obj);
}

function addOrConfirmFriend(initiator, confirmed, user) {
    if (user === null)
        return;
    var arguments =
        {            
            "userFrom": currentUser,
            "initiator": initiator,
            "confirmed": confirmed,
            "user": user
        };
    var obj =
        {
            "type": 3,
            "arguments": JSON.stringify(arguments)
        }
    //SearchFriend - server side method to be invoked
    connection.invoke(obj);
};

function createChat(name) {
    var arguments =
        {
            "userFrom": currentUser,
            "chatName": name
        };
    var obj =
        {
            "type": 4,
            "arguments": JSON.stringify(arguments)
        }
    // CreateChat - server side method to be invoked
    connection.invoke(obj);
}

function addToChat(chat, users) {
    var arguments =
        {
            "userFrom": currentUser,
            "chat":
            {
                "id": chat.id,
                "name": chat.name
            },
            "users": users
        };
    var obj =
        {
            "type": 5,
            "arguments": JSON.stringify(arguments)
        }
    // InviteToChat - server side method to be invoked
    connection.invoke(obj);
}

function confirmChatRequest(chat, confirmed) {
    var arguments =
        {            
            "userFrom": currentUser,
            "confirmed": confirmed,
            "chat": chat
        };
    var obj =
        {
            "type": 6,
            "arguments": JSON.stringify(arguments)
        }
    // ConfirmChat - server side method to be invoked
    connection.invoke(obj);
}