function allMessages() {
    return MESSAGES;
}

function addMessage(message) {
    var messages = allMessages();
    messages.push(message);
}

function createMessage(isNew, type, user, chat, message, incoming = true) {

    var date = typeof message.date === "string" ? new Date(message.date) : message.date;

    var mess = {
        "new": isNew,
        "type": type,
        "date": date,
        "user": user,
        "chat": chat,
        "text": message.text,
        "incoming": incoming
    }

    addMessage(mess);
    return mess;
}

function createPrivateMessageNew(user, message) {
    return createMessage(true, 0, user, null, message);
}

function createPrivateMessage(user, message, incoming = true) {
    return createMessage(false, 0, user, null, message, incoming);
}

function createChatMessageNew(user, chat, message) {
    return createMessage(true, 1, user, chat, message);
}

function createChatMessage(user, chat, message, incoming = true) {
    return createMessage(false, 1, user, chat, message, incoming);
}

function getPrivateMessages(user) {
    return allMessages().filter(mess => mess.type === 0 && mess.user.id === user.id);
    //.sort(a, b => a.date.getTime()>b.date.getTime());
}

function getChatMessages(chat) {
    return allMessages().filter(mess => mess.type === 1 && mess.chat.id === chat.id);    
}

function countNewPrivateMessages(user = null) {
    if (user == null)
        return allMessages().filter(mess => mess.type === 0 && mess.new).length;
    else
        return filterPrivateMessages(true, user).length;
}

function countNewChatMessages(chat = null) {
    if (chat == null)
        return allMessages().filter(mess => mess.type === 1 && mess.new).length;
    else 
        return filterChatMessages(true, chat).length;
}

function filterPrivateMessages(isNew, user) {
    return allMessages().filter(mess => mess.type === 0 && mess.new === isNew && mess.user.id === user.id);
}

function filterChatMessages(isNew, chat) {
    return allMessages().filter(mess => mess.type === 1 && mess.new === isNew && mess.chat.id === chat.id);
}

function changePrivateMessagesStatus(user) {
    filterPrivateMessages(true, user).forEach(mess => mess.new = false);
}

function changeChatMessagesStatus(chat) {
    filterChatMessages(true, chat).forEach(mess => mess.new = false);
}

function refillMessageList(mode, user, chat) {
    var messageList = document.getElementById("messages");
    clearListElement(messageList);

    if (mode === 0) {
        getPrivateMessages(user).forEach(mess => appendPrivateMessage(mess));
        changePrivateMessagesStatus(user);
        refreshNewPrivateMessages(user);
    } else {
        getChatMessages(chat).forEach(mess => appendChatMessage(mess));
        changeChatMessagesStatus(chat);
        refreshNewChatMessages(chat);
    }
}

function getPrivateMessageText(mess) {
    return mess.date.toLocaleTimeString() + " : " + mess.text;
}

function getChatMessageText(mess) {
    return mess.date.toLocaleTimeString() + " : " + mess.user.email + " : " + mess.text;
}

function createAndAppendPrivateMess(user, message, incoming=true) {
    var mess;
    if (currentMode === 0 && currentFriend!=null && currentFriend.id === user.id) {
        mess = createPrivateMessage(user, message, incoming);
        appendPrivateMessage(mess);   
    } else {
        createPrivateMessageNew(user, message);
    }
}

function createAndAppendChatMess(user, chat, message, incoming = true) {
    var mess;
    if (currentMode === 1 && currentChat != null && currentChat.id === chat.id) {
        mess = createChatMessage(user, chat, message, incoming);
        appendChatMessage(mess);
    } else
        createChatMessageNew(user, chat, message);
}

function refreshNewPrivateMessages(user) {
    refreshPrivateTab();
    refreshPrivateMessages(user);
}

function refreshNewChatMessages(chat) {
    refreshChatTab();
    refreshChatMessages(chat);
}

function refreshPrivateTab() {
    var tab = document.getElementById("friendsTab");
    var countNew = countNewPrivateMessages();
    if (countNew > 0)
        tab.innerText = "Friends " + "(" + countNew + ")";
    else
        tab.innerText = "Friends";
}

function refreshChatTab() {
    var tab = document.getElementById("chatsTab");
    var countNew = countNewChatMessages();
    if (countNew > 0)
        tab.innerText = "Chats " + "(" + countNew + ")";
    else
        tab.innerText = "Chats";
}

function refreshPrivateMessages(user) {   
    var countNew = countNewPrivateMessages(user);
    refreshNewCount(countNew, user.id);
}

function refreshChatMessages(chat) {
    var countNew = countNewChatMessages(chat);
    refreshNewCount(countNew, chat.id);
}

function refreshNewCount(count, id) {
    var countDiv;
    var item = document.getElementById(id);
    countDiv = item.getElementsByClassName("count");
    if (countDiv.length > 0)
        item.removeChild(countDiv[0]);
    if (count > 0) {
        countDiv = document.createElement("div");
        countDiv.className = "count";
        countDiv.appendChild(document.createTextNode("(" + count + ")"));
        item.appendChild(countDiv);
    }
}