import {Component, Input, ViewContainerRef} from '@angular/core';
import {CloseGuard, DialogRef, Modal, ModalComponent, overlayConfigFactory} from 'angular2-modal';
import { BSModalContext } from 'angular2-modal/plugins/bootstrap';
import {Chat} from "../../../models/Chat";
import {ChatsService} from "../../../services/chats.service";
import {SelectFriendsModal} from "../select-friends/select-friends";

export class CustomModalContext extends BSModalContext {
}

@Component({
    selector: 'select-chat',
    templateUrl: "./select-chat.html",
    styleUrls: ["./select-chat.css"],
})

export class SelectChatModal implements CloseGuard, ModalComponent<CustomModalContext> {

    context: CustomModalContext;
    chats : Chat[] = [];
    selectedChat : Chat;

    constructor(public dialog: DialogRef<CustomModalContext>,
                public chatsService : ChatsService,
                public modal: Modal) {
        this.context = dialog.context;
        dialog.setCloseGuard(this);
        this.chats = chatsService.getChats();
    }

    openSelectFriends() {
        this.modal.open(SelectFriendsModal,  overlayConfigFactory({chat : this.selectedChat}, BSModalContext));
    }

    selectChat(){
        if (this.selectedChat === undefined || this.selectedChat === null) return;
        this.openSelectFriends();
    }

    closeModal(){
        this.dialog.close(this.selectedChat);
    }
}
