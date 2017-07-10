import {Component, EventEmitter, Output} from '@angular/core';
import {ChatGlobalsService} from "../../../../services/chat-globals.service";
import {WsMessageService} from "../../../../services/wsMessage.service";
import {PrivateMessagesService} from "../../../../services/private-messages.service";
import {ChatMessagesService} from "../../../../services/chat-messages.service";

@Component({
    selector: 'message-input',
    templateUrl: './message-input.component.html',
    styleUrls: ['./message-input.component.css']
})
export class MessageInputComponent {

    messageInput : string = "";
    @Output() messageSent = new EventEmitter<void>();

    constructor(private chatGlobals : ChatGlobalsService
                ,private wsMessageService : WsMessageService
                ,private privateMessagesService : PrivateMessagesService
                ,private chatMessagesService: ChatMessagesService){
    }

    sendMessage(){
        if (this.messageInput === "" ) return;
        if (this.chatGlobals.privateMode() && !this.chatGlobals.currentFriend) return;
        if (!this.chatGlobals.privateMode() && !this.chatGlobals.currentChat) return;

        let mess = {
            id : -1,
            text : this.messageInput,
            date : new Date
        };
        this.wsMessageService.sendMessage(mess);

        if (this.chatGlobals.privateMode())
            this.privateMessagesService.createMessage(this.chatGlobals.currentUser, this.chatGlobals.currentFriend, mess, false, false);
        else
            this.chatMessagesService.createMessage(this.chatGlobals.currentChat, this.chatGlobals.currentUser, mess, false, false);

        this.messageInput = "";
        this.messageSent.emit();
    }
}