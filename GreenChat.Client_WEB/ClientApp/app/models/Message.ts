import { User } from "./User";
import { Chat } from "./Chat";
import {MessageContent} from "./MessageContent";

export abstract class Message {

    isNew: boolean;
    userFrom: User;
    incoming: boolean;
    content : MessageContent;

    constructor(userFrom: User, message : any, isNew :boolean, incoming: boolean){
        this.isNew = isNew;
        this.userFrom = userFrom;
        this.content = MessageContent.create(message);
        this.incoming = incoming;
    }

    getTextWithDate() : string {
        return this.content.date.toLocaleTimeString() + " : ";
    }

    getText() : string{
       return this.userFrom.email + " : " + this.content.text;
    }
}

export class PrivateMessage extends Message {
    userTo : User

    constructor(userFrom: User, userTo: any, message : any, isNew: boolean, incoming: boolean) {
        super(userFrom, message, isNew, incoming);
        this.userTo = userTo;
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

    constructor(chat: Chat, userFrom: User, message : any, isNew: boolean, incoming: boolean) {
        super(userFrom, message, isNew, incoming);
        this.chat = chat;
    }

    getText() : string{
        return this.incoming ? this.userFrom.email + " : " + this.content.text : this.content.text;
    }

    getTextWithDate(): string {
        return super.getTextWithDate() +  this.userFrom.email + " : " + this.content.text;
    }
}