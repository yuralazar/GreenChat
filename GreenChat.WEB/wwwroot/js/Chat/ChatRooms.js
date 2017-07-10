function createChatUsersSelect(list, element) {

    var selectUserToChat = document.createElement("select");
    selectUserToChat.id = "selectUserToChat";
    selectUserToChat.className = "form-control";

    for (var i = 0; i < list.length; i++) {
        var user = list[i];
        if (user.potential) continue;

        appendFriendsSelect(selectUserToChat, user);
    }

    insertAfter(selectUserToChat, element);

    return selectUserToChat;
}

function ChatConfirmationButtons(chat, messageItem) {

    var btnDiv = document.createElement("div");
    btnDiv.className = "green-message-btn";
    insertAfter(btnDiv, messageItem);

    var confirmButton = createAndAppendButton("chatRequestConfirm" + chat.id, "Confirm", messageItem);
    var declineButton = createAndAppendButton("chatRequestDecline" + chat.id, "Decline", confirmButton);

    btnDiv.appendChild(confirmButton);
    btnDiv.appendChild(declineButton);

    confirmButton.addEventListener("click", confirmChatRequest.bind(null, chat, true));
    declineButton.addEventListener("click", confirmChatRequest.bind(null, chat, false));
    confirmButton.addEventListener("click",
        function () {
            removeFromParent(declineButton);
            removeFromParent(confirmButton);
        }
    );
    declineButton.addEventListener("click",
        function () {
            removeFromParent(declineButton);
            removeFromParent(confirmButton);
        }
    );
}

function showChatConfirmationButtons(list) {
    for (var i = 0; i < list.length; i++) {
        var row = list[i];
        var messageText = "User " + row.userFrom.email + " invites you to chat room '" + row.chat.name + "'";
        var messageItem = appendItem(messageList, messageText, " green-info");
        ChatConfirmationButtons(row.chat, messageItem);
    }
}

function refillChatList(list) {

    for (var j = 0; j < list.length; j++) {
        var chat = list[j];
        appendChatList(chat);
    }
}

function appendChatList(chat) {
    var chatList = document.getElementById("chatList");
    var item = document.createElement("li");
    item.id = chat.id;
    item.className = "green-list-item";
    var div = document.createElement("div");
    div.className = chatsStyle();
    div.addEventListener("click", activateReciever.bind(null, 1, chat));
    div.appendChild(document.createTextNode(chat.name));
    item.appendChild(div);
    chatList.appendChild(item);
};

function addChat(chat) {
    chatList.push(chat);
}

function chatsStyle(active = false) {
    var style = "green-friend-offline";
    var styleActive = active ? "active" : "";
    return style + " " + styleActive;
}