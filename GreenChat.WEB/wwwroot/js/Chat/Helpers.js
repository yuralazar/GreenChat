function changeListItemStyleActive(active, id) {
    var friendsListItem = document.getElementById(id);
    if (friendsListItem != null) {
        var div = friendsListItem.getElementsByTagName("div")[0];
        if (active)
            div.className = div.className + " active";
        else
            div.className = div.className.replace(" active","");
    }
}

function changeListItemStyle(style, id) {
    var friendsListItem = document.getElementById(id);
    if (friendsListItem != null) {
        var div = friendsListItem.getElementsByTagName("div")[0];
        div.className = style;
    }
}

function createAndAppendButton(id, text) {

    var button = document.createElement("button");
    button.className = "btn btn-success";
    button.id = id;
    var textNode = document.createTextNode(text);
    button.appendChild(textNode);

    return button;
}

function insertAfter(newNode, referenceNode) {
    referenceNode.parentNode.insertBefore(newNode, referenceNode.nextSibling);
}

function removeFromParent(node) {
    node.parentNode.removeChild(node);
}

function activateReciever(mode, model) {

    if (currentMode === 0 && currentFriend!==null)
        changeListItemStyleActive(false, currentFriend.id);
    else if (currentMode === 1 && currentChat !== null)
        changeListItemStyleActive(false, currentChat.id);

    currentMode = mode;

    if (mode === 0) {
        currentFriend = model;
        changeListItemStyleActive(true, currentFriend.id);
        refillMessageList(mode, currentFriend, null);
    }
    else {
        currentChat = model;
        changeListItemStyleActive(true, currentChat.id);
        refillMessageList(mode, null, currentChat);
    }
}

function clearSelectOptions(select) {
    var options = select.options;
    var optionsCount = options.length;

    for (var i = optionsCount - 1; i >= 0; i--) {
        select.remove(i);
    }
}

function clearChildNode(node, id) {
    var elem = document.getElementById(id);
    if (elem !== null)
        node.removeChild(elem);
}

function getMessagesElement() {
    return document.getElementById("messages");
}

function appendPrivateMessage(messageObj) {
    var messageList = getMessagesElement();
    var text = getPrivateMessageText(messageObj);
    return appendItem(messageList, text, "", !messageObj.incoming);
}

function appendChatMessage(messageObj) {
    var messageList = getMessagesElement();
    var text = getChatMessageText(messageObj);
    return appendItem(messageList, text, "", !messageObj.incoming);
}

function appendWarningMessage(message) {
    var messageList = getMessagesElement();
    return appendItem(messageList, message, " green-warn");
}

function appendInfoMessage(message) {
    var messageList = getMessagesElement();
    return appendItem(messageList, message, " green-info");
}

function appendItem(list, message, styleClass = "", self = false) {
    var item = document.createElement("li");
    item.className = "green-message" + (self ? " self":"");
    var messdiv = document.createElement("div");
    messdiv.className = "green-message-text" + styleClass;
    messdiv.appendChild(document.createTextNode(message));
    item.appendChild(messdiv);
    list.appendChild(item);

    return item;
}

function clearListElement(listElement) {
    var liElems = listElement.getElementsByTagName("li");
    for (var i = liElems.length - 1; i >= 0; i--) {
        listElement.removeChild(liElems[i]);
    }
}
