import { Injectable } from '@angular/core';
import {ChatMessage} from "../models/Message";
import {ChatMessagesContainer} from "../models/ChatMessagesContainer";
import {Chat} from "../models/Chat";
import {MessagesService} from "./message-service";
import {FriendsService} from "./friends.service";
import {User} from "../models/User";
import {ChatGlobalsService} from "./chat-globals.service";
import {WsMessageService} from "./wsMessage.service";

@Injectable()
export class ChatMessagesService extends MessagesService{

    constructor(private chatGlobals : ChatGlobalsService,
                private wsMessageService : WsMessageService) {
        super();
    }

    addMessageContainer(chat: Chat) {
        let container =  new ChatMessagesContainer(chat);
        this.messagesContainers.push(container);
        return container;
    }

    createMessage(chat : Chat, userFrom : User, message : any,
                  isNew: boolean, incoming: boolean) : ChatMessage {
        let mess = new ChatMessage(chat, userFrom, message, isNew, incoming);
        this.addMessage(chat, mess);
        return message;
    }

    loadMessages(chat: any|Chat, messages : any[], historyLoaded:boolean = false) {
        let currentUser = this.chatGlobals.currentUser;
        messages.forEach(mess => {
            this.createMessage(chat, mess.userFrom, mess.message, false,
                                !this.chatGlobals.isCurrentUser(mess.userFrom))
        })
        let container = this.getContainerByOwner(chat);
        container.historyLoaded = historyLoaded;
    }

    getHistory(chat: Chat, count: number, startDate : Date = new Date) {
        this.wsMessageService.getChatMessages(chat, count, startDate);
    }

    deleteUnreadMessages(chat: Chat) {
        this.wsMessageService.deleteChatMessages(chat);
    }
}
