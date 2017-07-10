import {Component, Input, ViewContainerRef} from '@angular/core';
import {CloseGuard, DialogRef, Modal, ModalComponent, overlayConfigFactory} from 'angular2-modal';
import { BSModalContext } from 'angular2-modal/plugins/bootstrap';
import {FriendsService} from "../../../services/friends.service";
import {ChatService} from "../../../services/chat.service";
import {WsMessageService} from "../../../services/wsMessage.service";
import {User} from "../../../models/User";
import {ChatGlobalsService} from "../../../services/chat-globals.service";
import {Chat} from "../../../models/Chat";
import {ChatsService} from "../../../services/chats.service";

export class CustomModalContext extends BSModalContext {
    chat : Chat;
}

@Component({
    selector: 'select-friends',
    templateUrl: "./select-friends.html",
    styleUrls: ["./select-friends.css"],
})

export class SelectFriendsModal implements CloseGuard, ModalComponent<CustomModalContext> {

    context: CustomModalContext;
    friends : User[] = [];
    selectedFriends : User[] = [];

    constructor(public dialog: DialogRef<CustomModalContext>,
                private friendsService : FriendsService,
                private wsMessageService : WsMessageService) {
        this.context = dialog.context;
        dialog.setCloseGuard(this);

        this.friends = this.getFriendsNotInChat();
    }

    private getFriendsNotInChat() {
        let userIds = this.context.chat.users.map(usr => usr.id);
        return this.friendsService.getFriends().filter(usr=> userIds.indexOf(usr.id) === -1);
    }

    sendChatRequest(){
        if (this.selectedFriends.length > 0) {
            this.wsMessageService.addToChat(this.context.chat, this.selectedFriends);
            this.addSelectedFriendsToChat();
            this.closeModal();
        }
    }

    onChange(user: User, checked:boolean){
        if (checked)
            this.addFriend(user);
        else
            this.removeFriend(user);
    }

    addFriend(user : User){
        this.selectedFriends.push(user);
    }

    removeFriend(user : User){
        let index = this.selectedFriends.indexOf(user);
        if (index > -1) {
            this.selectedFriends.splice(index, 1);
        }
    }

    closeModal(){
        this.dialog.close();
    }

    private addSelectedFriendsToChat() {
        this.selectedFriends.forEach(fr => this.context.chat.users.push(fr));
    }
}
