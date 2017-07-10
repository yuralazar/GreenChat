import { Injectable } from '@angular/core';
import {Message, PrivateMessage} from "../models/Message";
import {User} from "../models/User";
import {PrivateMessagesContainer} from "../models/PrivateMessagesContainer";
import {MessagesService} from "./message-service";
import {WsMessageService} from "./wsMessage.service";
import {MessageContent} from "../models/MessageContent";

@Injectable()
export class PrivateMessagesService extends MessagesService{

    constructor(private wsMessageService : WsMessageService) {
        super();
    }

    addMessageContainer(user : User)
    {
        let container = new PrivateMessagesContainer(user);
        this.messagesContainers.push(container);
        return container;
    }

    createMessage(userFrom : User, userTo : User,
                  message : any,
                  isNew: boolean, incoming: boolean) : PrivateMessage {
        let mess = new PrivateMessage(userFrom, userTo, message, isNew, incoming);
        this.addMessage(incoming ? userFrom : userTo, mess);
        return message;
    }

    loadMessages(userFrom: any|User, messages : any[], historyLoaded:boolean = false) {
        messages.forEach(mess => {
            this.createMessage(mess.userFrom, mess.userTo, mess.message,
                    false, mess.userFrom.id === userFrom.id)
        })
        let container = this.getContainerByOwner(userFrom);
        container.historyLoaded = historyLoaded;
    }

    getHistory(user: User , count: number, startDate : Date = new Date) {
        this.wsMessageService.getPrivateMessages(user, count, startDate);
    }

    deleteUnreadMessages(currentFriend: User) {
        this.wsMessageService.deletePrivateMessages(currentFriend);
    }
}
