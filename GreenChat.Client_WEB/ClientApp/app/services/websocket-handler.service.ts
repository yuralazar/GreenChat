/*import { Injectable } from '@angular/core';

@Injectable()
export class WebSocketHandlerService{

    userConnectionState(args : any, online : boolean) {
        if (!args)
            return;
        var user = args.user;
        console.log(user.email + " is connected (" + new Date() + ")");
        //markFriendOnline(user);
    }

    initialInfo(args : any) {
        friendsList = args.data.friends;
        chatList = args.data.chatRooms;
        var chatRequestList = args.data.chatRequests;
        refillFriendsList(friendsList);
        refillChatList(chatList);

        showFriendsConfirmationButtons(friendsList);
        showChatConfirmationButtons(chatRequestList);
    }
    privateMessage(args : any) {
        var dataArg = args.data;
        createAndAppendPrivateMess(dataArg.userFrom, dataArg.message);
        refreshNewPrivateMessages(dataArg.userFrom);
        markFriendOnline(dataArg.userFrom);
    }
    chatMessage(args : any) {
        var dataArg = args.data;
        createAndAppendChatMess(dataArg.userFrom, dataArg.chat, dataArg.message);
        refreshNewChatMessages(dataArg.chat);
        markFriendOnline(dataArg.userFrom);
    }
    userFound(args : any) {
        var dataArg = args.data;
        var messageText;

        if (dataArg.user.id === null) {
            messageText = "User " + dataArg.user.email + " not found!";
            appendWarningMessage(messageText);
        } else {
            messageText = "User " + dataArg.user.email + " found! You can add him to friends list!";
            var messageItem = appendItem(messageList, messageText, " green-info");

            var searchedUser = dataArg.user;

            var btnDiv = document.createElement("div");
            btnDiv.className = "green-message-btn";
            insertAfter(btnDiv, messageItem);
            var addButton = createAndAppendButton("addFriend", "Add", messageItem);
            var cancelAddingButton = createAndAppendButton("cancelAddingButton", "Cancel", addButton);
            btnDiv.appendChild(addButton);
            btnDiv.appendChild(cancelAddingButton);

            addButton.addEventListener("click", addOrConfirmFriend.bind(null, true, true, searchedUser));
            addButton.addEventListener("click",
                function () {
                    appendItem(messageList, "Now wait for user to confirm frendship", " green-info");
                    removeFromParent(cancelAddingButton);
                    removeFromParent(addButton);
                });
            cancelAddingButton.addEventListener("click",
                function () {
                    removeFromParent(cancelAddingButton);
                    removeFromParent(addButton);
                });
        }
        console.log(messageText);
    }
    friendRequest(args : any) {
        var dataArg = args.data;
        var messageText = "User " + dataArg.userFrom.email + " wants to become your friend";
        var messageItem = appendItem(messageList, messageText, " green-info");
        var confirmationUser = dataArg.userFrom;
        FriendConfirmationButtons(confirmationUser, messageItem);

        console.log(messageText);
    }
    friendConfirmed(args : any) {
        var dataArg = args.data;
        var messageText;
        if (dataArg.confirmed) {
            messageText = "User " + dataArg.userFrom.email + " is now your friend";
            appendFriendsList(dataArg.userFrom, true);
            addFriend(dataArg.userFrom, true);
            appendInfoMessage(messageText);
        } else {
            messageText = "User " + dataArg.userFrom.email + " declined you friendship request";
            appendWarningMessage(messageText);
        }
        console.log(messageText);
    }
    chatCreated(args : any) {
        var dataArg = args.data;
        var messageText = "Chat " + dataArg.chat.name + " created";
        var messageItem = appendInfoMessage(messageText);
        appendChatList(dataArg.chat);

        var selectUserToChat = createChatUsersSelect(friendsList, messageItem);

        var btnDiv = document.createElement("div");
        btnDiv.className = "green-message-btn";
        insertAfter(btnDiv, selectUserToChat);

        var addFriendToChat = createAndAppendButton("addFriendToChat", "Add", selectUserToChat);
        var closeButton = createAndAppendButton("closeAddChatList", "Close", addFriendToChat);
        btnDiv.appendChild(addFriendToChat);
        btnDiv.appendChild(closeButton);

        addFriendToChat.addEventListener("click", addToChatAction.bind(null, dataArg.chat));
        closeButton.addEventListener("click",
            function () {
                removeFromParent(closeButton);
                removeFromParent(addFriendToChat);
                removeFromParent(selectUserToChat);
            });

        console.log(messageText);
    }

    chatRequest(args : any) {
        var dataArg = args.data;
        var messageText = "User " + dataArg.userFrom.email + " invites you to chat room '" + dataArg.chat.name + "'";
        var messageItem = appendInfoMessage(messageText);
        var chat = dataArg.chat;
        ChatConfirmationButtons(chat, messageItem);
        console.log(messageText);
    }

    chatConfirmed(args : any) {
        var dataArg = args.data;
        var messageText;
        if (dataArg.confirmed) {
            if (dataArg.userFrom.id === currentUser.id) {
                appendChatList(dataArg.chat);
                messageText = "Chat '" + dataArg.chat.name + "' added to chat list";

            } else {
                messageText = "User " + dataArg.userFrom.email + " joined chat room '" + dataArg.chat.name + "'";
            }
            appendInfoMessage(messageText);
        } else {
            messageText = "User " + dataArg.userFrom.email + " refused to join chat room '" + dataArg.chat.name + "'";
            appendWarningMessage(messageText);
        }
        console.log(messageText);
    }    
}
*/