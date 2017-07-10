import {Message} from "./Message";
import {Chat} from "./Chat";
import {User} from "./User";

export abstract class MessagesContainer {

    // user or chat to which this container belong
    historyLoaded : boolean = false;
    private messages : Message[] = [];

    constructor(public owner : User|Chat) {}

    getStartDate() {
        let messages = this.getMessages();
        if (messages.length > 0)
            return messages[0].content.date;
        return new Date;
    }

    getMessages() : Message[] {
        return this.messages.sort((a, b) => a.content.date.getTime() - b.content.date.getTime());
    }

    addMessage(message: Message) {
        if (!this.messages.find(m => m.content.id === message.content.id && m.incoming))
            this.messages.push(message);
    }

    countNew() : number{
        return this.messages.filter(mess => mess.isNew).length;
    }

    unmarkNew() {
        this.messages.forEach(m => {if(m.isNew) m.isNew = false});
    }
}
