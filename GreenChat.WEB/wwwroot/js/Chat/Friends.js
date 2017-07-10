function FriendConfirmationButtons(user, messageItem, isOnline = false) {

    var btnDiv = document.createElement("div");
    btnDiv.className = "green-message-btn";
    insertAfter(btnDiv, messageItem);

    var confirmButton = createAndAppendButton("confirmFriend" + user.email, "Confirm", messageItem);
    var declineButton = createAndAppendButton("declineFriend" + user.email, "Decline", confirmButton);

    btnDiv.appendChild(confirmButton);
    btnDiv.appendChild(declineButton);

    confirmButton.addEventListener("click", addOrConfirmFriend.bind(null, false, true, user));
    declineButton.addEventListener("click", addOrConfirmFriend.bind(null, false, false, user));
    confirmButton.addEventListener("click",
        function () {
            appendFriendsList(user, isOnline);
            addFriend(user);
            removeFromParent(declineButton);
            removeFromParent(confirmButton);
        });
    declineButton.addEventListener("click",
        function () {
            removeFromParent(declineButton);
            removeFromParent(confirmButton);
        });
}

function showFriendsConfirmationButtons(list) {
    for (var i = 0; i < list.length; i++) {
        var user = list[i];
        if (!user.potential) continue;
        var messageText = "User " + user.email + " wants to become your friend";
        var messageItem = appendItem(messageList, messageText, " green-info");
        FriendConfirmationButtons(user, messageItem, user.online);
    }
}

function refillFriendsList(list) {

    for (var j = 0; j < list.length; j++) {
        var user = list[j];
        if (user.potential) continue;
        appendFriendsList(user);
    }
}

function appendFriendsList(user) {
    var friendsList = document.getElementById("friendsList");
    var item = document.createElement("li");
    item.id = user.id;
    item.className = "green-list-item";
    var div = document.createElement("div");
    div.className = friendsStyle(user.online);
    div.addEventListener("click", activateReciever.bind(null, 0, user));
    div.appendChild(document.createTextNode(user.email));
    item.appendChild(div);
    friendsList.appendChild(item);
};

function appendFriendsSelect(select, user) {
    var options = select.options;
    var item = document.createElement("option");
    item.id = user.id;
    item.text = user.email;
    options.add(item);
};

function addFriend(user) {
    friendsList.push(user);
}

function friendsStyle(online, active = false) {
    var style = online ? "green-friend-online" : "green-friend-offline";
    var styleActive = active ? "active" : "";
    return style + " " + styleActive;
}

function markFriendOnline(user) {
    changeListItemStyle(friendsStyle(true, (currentFriend != null && currentFriend.id === user.id)), user.id);
}

function markFriendOffline(user) {
    changeListItemStyle(friendsStyle(false), user.id);
}