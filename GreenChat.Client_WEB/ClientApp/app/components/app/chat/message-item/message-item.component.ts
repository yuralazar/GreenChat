import {Component, Injector, Input} from '@angular/core';
import {ChatMessage, Message, PrivateMessage} from "../../../../models/Message";
import {PrivateMessagesService} from "../../../../services/private-messages.service";
import {ChatMessagesService} from "../../../../services/chat-messages.service";
import {MessageStatus} from "../../../../models/MessageState";

@Component({
    selector: 'message-item',
    templateUrl: './message-item.component.html',
    styleUrls: ['./message-item.component.css']
})
export class MessageItemComponent {

    @Input() message : Message;

    constructor(private privateMessagesService: PrivateMessagesService
               ,private chatMessagesService: ChatMessagesService)
    {
    }

    isChatMessage(message : Message) : boolean{
        return message instanceof ChatMessage;
    }

    showStatus(message : Message) : boolean{
        if(message instanceof PrivateMessage){
            if (!message.incoming){
                if (message.status == MessageStatus.Sent || message.status == MessageStatus.Delivered)
                    return true;
            }
        }
        return false;
    }

    unmarkMessage(message : Message){
        if (!(message.incoming && message.isNew)) return;

        let service;
        let owner;
        if (this.isChatMessage(message)){
            service = this.chatMessagesService;
            owner = (message as ChatMessage).chat;
        }
        else{
            service = this.privateMessagesService;
            owner = message.userFrom;
        }

        service.changeMessageStatus(message.content.id, owner, MessageStatus.Seen);
        service.changeMessageIsNew(message);
    }
}