import {Component, Input} from '@angular/core';
import {CloseGuard, DialogRef, ModalComponent} from 'angular2-modal';
import { BSModalContext } from 'angular2-modal/plugins/bootstrap';
import {FriendsService} from "../../../services/friends.service";
import {ChatService} from "../../../services/chat.service";
import {WsMessageService} from "../../../services/wsMessage.service";
import {User} from "../../../models/User";
import {ChatGlobalsService} from "../../../services/chat-globals.service";

export class CustomModalContext extends BSModalContext {
}

@Component({
    selector: 'create-chat',
    templateUrl: "./create-chat.html",
    styleUrls: ["./create-chat.css"],
})

export class CreateChatModal implements CloseGuard, ModalComponent<CustomModalContext> {

    context: CustomModalContext;
    chatName : string;

    constructor(public dialog: DialogRef<CustomModalContext>,
                private chatService : ChatService,
                private wsMessageService : WsMessageService) {

        this.context = dialog.context;
        dialog.setCloseGuard(this);
    }

    createChatRoom(){
        if (this.chatName === "") return;
        this.wsMessageService.createChat(this.chatName);
        this.closeModal();
    }

    closeModal(){
        this.dialog.close();
    }
}
