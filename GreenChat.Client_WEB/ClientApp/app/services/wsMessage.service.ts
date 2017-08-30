import { Injectable } from '@angular/core';
import {User} from "../models/User";
import {Message} from "../models/Message";
import {Chat} from "../models/Chat";
import {ChatGlobalsService} from "./chat-globals.service";
import {ChatService} from "./chat.service";
import {MessageStatus} from "../models/MessageState";

@Injectable()
export class WsMessageService{

    blockedMessages : {type: number, args : any}[] = [];

    constructor(private chatGlobals : ChatGlobalsService,
                private chatService : ChatService) {
    }

    sendMessage( message: {text: string, date:Date}, newId : number) {
        if (this.chatGlobals.privateMode()){
            return this.sendPrivate(this.chatGlobals.currentFriend, message, newId);
        }else {
            return this.sendChat(this.chatGlobals.currentChat, message, newId);
        }
    }

    public sendPrivate(user : User, message: {text: string, date:Date}, newId : number) {
        let args =
            {
                "userFrom": this.chatGlobals.currentUser,
                "message": message,
                "userTo": user,
                "idNew" : newId
            };
        return this.sendJsonMessage(0, args);
    }

    public sendChat(chat: Chat, message: {text: string, date:Date}, newId : number) {
        let args =
            {
                "userFrom": this.chatGlobals.currentUser,
                "message": message,
                "chat": chat,
                "idNew" : newId
            };
        return this.sendJsonMessage(1, args);
    }

    public searchFriend(email : string) :string {
        let args =
            {
                userFrom : this.chatGlobals.currentUser,
                searchEmail : email
            };
        return this.sendJsonMessage(2, args);
    }

    addOrConfirmFriend(user : User, initiator: boolean, confirmed:boolean ) {
        let args =
            {
                "userFrom": this.chatGlobals.currentUser,
                "initiator": initiator,
                "confirmed": confirmed,
                "user": user
            };
        return this.sendJsonMessage(3, args);
    };

    createChat(name) {
        let args =
        {
            "userFrom": this.chatGlobals.currentUser,
            "chatName": name
        };
        return this.sendJsonMessage(4, args);
    }

    addToChat(chat: Chat, users: User[]) {

        let args =
            {
                "userFrom": this.chatGlobals.currentUser,
                "chat": chat,
                "users": users
            };

        return this.sendJsonMessage(5, args);

    }

    confirmChatRequest(chat : Chat, user : User, confirmed :boolean ) {
        let args =
            {
                "userFrom": this.chatGlobals.currentUser,
                "confirmed": confirmed,
                "chat": chat,
                "invitor" : user
            };

        return this.sendJsonMessage(6, args);
    }

    deletePrivateMessages(user: User) {
        let args =
            {
                "userFrom": user,
                "userTo": this.chatGlobals.currentUser
            };
        return this.sendJsonMessage(7, args);
    }

    deleteChatMessages(chat: Chat) {
        let args =
            {
                "chat": chat,
                "userTo": this.chatGlobals.currentUser
            };
        return this.sendJsonMessage(8, args);
    }

    getPrivateMessages(currentFriend: User, count: number, startDate: Date) {
        let args =
            {
                "userFrom": this.chatGlobals.currentUser,
                "sender": currentFriend,
                "count": count,
                "startDate": startDate
            };
        return this.sendJsonMessage(9, args);
    }

    getChatMessages(chat: Chat, count: number, startDate: Date) {
        let args =
            {
                "userFrom": this.chatGlobals.currentUser,
                "chat": chat,
                "count": count,
                "startDate": startDate
            };
        return this.sendJsonMessage(10, args);
    }

    privateMessageStatus(user : User, messId : number, status : MessageStatus) {
        let args =
            {
                "userTo": this.chatGlobals.currentUser,
                "userFrom": user,
                "id" : messId,
                "status" : status
            };
        return this.sendJsonMessage(11, args);
    }

    chatMessageStatus(currentChat: Chat, messId: number, status: MessageStatus) {
        let args =
            {
                "userFrom": this.chatGlobals.currentUser,
                "chat": currentChat,
                "id" : messId,
                "status" : status
            };
        return this.sendJsonMessage(12, args);
    }

    private sendJsonMessage(type : number, args : any) : string{
        let obj =
            {
                type: type,
                arguments: JSON.stringify(args)
            }
        let mess = JSON.stringify(obj);
        this.chatService.messageData.next(mess);
        return mess;
    }

}
