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
    selector: 'search-friend',
    templateUrl: "./search-friend.html",
    styleUrls: ["./search-friend.css"],
})

export class SearchFriendModal implements CloseGuard, ModalComponent<CustomModalContext> {

    enableLogging : boolean = true;
    context: CustomModalContext;
    friendFound: boolean = false;
    searchEmail: string = "";
    infoNumb : number;

    constructor(public dialog: DialogRef<CustomModalContext>,
                private chatGlobals : ChatGlobalsService,
                private friendsService: FriendsService,
                private wsMessageService : WsMessageService) {
        this.context = dialog.context;
        dialog.setCloseGuard(this);
    }

    searchFriend(){
        this.infoNumb = 0;
        if (this.searchEmail === "") return;
        if (this.friendsService.getFriendByEmail(this.searchEmail) !== undefined) {
            this.infoNumb = 1;
            return;
        }
        let currentUser = this.chatGlobals.currentUser;
        if (this.searchEmail === currentUser.email) {
            this.infoNumb = 3;
            return;
        }
        this.wsMessageService.searchFriend(this.searchEmail);
        this.closeModal();
    }

    closeModal(){
        this.dialog.close();
    }
}
