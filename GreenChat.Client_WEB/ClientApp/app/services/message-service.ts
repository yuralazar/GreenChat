import {ChatMessage, Message, PrivateMessage} from "../models/Message";
import {MessagesContainer} from "../models/MessagesContainer";
import {FriendsService} from "./friends.service";
import {User} from "../models/User";
import {Chat} from "../models/Chat";
import {isUndefined} from "util";

export abstract class MessagesService{

    messagesContainers : MessagesContainer[] = [];

    constructor(){
    }

    abstract addMessageContainer(owner : User|Chat) : MessagesContainer;
    abstract getHistory(owner : User|Chat, count : number, startDate? : Date);
    abstract deleteUnreadMessages(owner : User|Chat);
    abstract loadMessages(owner: any|User|Chat, messages : any[], historyLoaded?: boolean);

    getMessages(owner : User|Chat) : Message[]{
        let container = this.getContainerByOwner(owner);
        return container.getMessages();
    }

    filterNew(owner : User|Chat) : Message[]{
        return this.getMessages(owner).filter(mess => mess.isNew)
    }

    changeStatus(owner : User|Chat){
        this.filterNew(owner).forEach(mess => mess.isNew = false);
    }

    getContainerByOwner(owner: User|Chat) {
        let container = this.messagesContainers.find(cont => cont.owner.id === owner.id);
        if (container === undefined)
            container = this.addMessageContainer(owner);
        return container
    }

    countNew(): number {
        let count = 0;
        this.messagesContainers.forEach(container => count += container.countNew())
        return count;
    }

    countNewByOwner(owner : User|Chat) {
        return this.getContainerByOwner(owner).countNew();
    }

    addMessage(owner : User|Chat, message : Message) {
        let container = this.getContainerByOwner(owner);
        container.addMessage(message);
    }
}
