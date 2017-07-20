import { User } from "./User";
import { Chat } from "./Chat";
import {MessageContent} from "./MessageContent";
import {MessageStatus} from "./MessageState";

export abstract class Message {

    isNew: boolean;
    userFrom: User;
    incoming: boolean;
    content : MessageContent;
    status : MessageStatus;
    idNew : number;

    private static newId : number = 0;
    static getId() : number{
        return this.newId++;
    }

    constructor(userFrom: User, message : any, isNew :boolean, incoming: boolean, status : MessageStatus = null){
        this.isNew = isNew;
        this.userFrom = userFrom;
        this.content = MessageContent.create(message);
        this.incoming = incoming;
        this.status = status;
        this.idNew = message.idNew;
    }

    abstract getStatusText() : string;

    getTextWithDate() : string {
        return this.content.date.toLocaleTimeString() + " : ";
    }

    getText() : string{
       return this.userFrom.email + " : " + this.content.text;
    }
}

export class PrivateMessage extends Message {

    userTo : User;

    constructor(userFrom: User, userTo: any, message : any
        , isNew: boolean, incoming: boolean, status : MessageStatus = null) {
        super(userFrom, message, isNew, incoming, status);
        this.userTo = userTo;
    }

    getStatusText() : string {
        switch (this.status){
            case (MessageStatus.Delivered) : return "Delivered";
            case (MessageStatus.Sent) : return "Sent";
        }
    }

    getTextWithDate(): string {
        return super.getTextWithDate() + this.content.text;
    }

    getText() : string{
        return this.content.text;
    }
}

export class ChatMessage extends Message {

    chat : Chat;

    constructor(chat: Chat, userFrom: User, message : any
            , isNew: boolean, incoming: boolean, status : MessageStatus = null) {
        super(userFrom, message, isNew, incoming, status);
        this.chat = chat;
    }

    getStatusText(): string {
        return "";
    }

    getText() : string{
        return this.incoming ? this.userFrom.email + " : " + this.content.text : this.content.text;
    }

    getTextWithDate(): string {
        return super.getTextWithDate() +  this.userFrom.email + " : " + this.content.text;
    }
}