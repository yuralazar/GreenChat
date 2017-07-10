import {Component, EventEmitter, Output, ViewChild, ViewContainerRef} from '@angular/core';
import {FriendsService} from "../../../../services/friends.service";
import {User} from "../../../../models/User";
import {Chat} from "../../../../models/Chat";
import {ChatsService} from "../../../../services/chats.service";
import {DropItem} from "../../../../models/DropItem";
import {Modal} from "angular2-modal";

import {SearchFriendModal} from "../../../modals/search-friend/search-friend";
import {ChatGlobalsService} from "../../../../services/chat-globals.service";
import {PrivateMessagesService} from "../../../../services/private-messages.service";
import {ChatMessagesService} from "../../../../services/chat-messages.service";
import {CreateChatModal} from "../../../modals/create-chat/create-chat";
import {SelectChatModal} from "../../../modals/select-chat/select-chat";
import {Tab} from "../../../../interfaces/tab.interface";
import {TabsComponent} from "../tabs-components/tabs/tabs.component";

@Component({
    selector: 'friends-chats-list',
    templateUrl: './friends-chats-list.component.html',
    styleUrls: ['./friends-chats-list.component.css']
})
export class FriendsAndChatsComponent {
    friends : User[] = [];
    chats : Chat[] = [];
    friendsMenuItems : DropItem[] = [new DropItem("Search friend", SearchFriendModal, {})];
    chatsMenuItems : DropItem[] =[
                                    new DropItem("Create chat", CreateChatModal, {}),
                                    new DropItem("Add friends to chat", SelectChatModal, {})
                                 ];
    @Output() ownerActivated = new EventEmitter<void>();
    @Output() tabActivated = new EventEmitter<string>();
    showBusy : boolean = false;

    constructor(private privateMessagesService : PrivateMessagesService,
                private chatMessagesService : ChatMessagesService,
                private chatGlobals : ChatGlobalsService,
                private friendsService : FriendsService,
                private chatsService : ChatsService,
                public modal: Modal)
    {
    }

    countNewPrivate(user : User){
        let count = this.privateMessagesService.countNewByOwner(user);
        return count;
    }

    countNewChat(chat : Chat){
        let count = this.chatMessagesService.countNewByOwner(chat);
        return count;
    }

    fillFriendsList(friends: any[]) {
        this.friends = [];
        friends.forEach(fr => {
            if (!fr.potential)
                this.createAndAppendFriend(fr)
        });
        this.friends = this.friendsService.getFriends();
    }

    createAndAppendFriend(friend: any) {
        this.friendsService.createFriend(friend.id, friend.email, friend.online, friend.potential)
    }

    fillChatsList(chats: any[], chatsUsers: any) {
        this.chats = [];
        chats.forEach(ch => {
            let chatUsers = chatsUsers.filter(chatUsers => chatUsers.chat.id === ch.id);
            if (chatUsers.length > 0) {
                let users = chatUsers[0].users;
                this.createAndAppendChat(ch, users);
            }
        });
        this.chats = this.chatsService.getChats();
    }

    createAndAppendChat(chat : any, users: any) {
        this.chatsService.createChat(chat.id, chat.name, users);
    }

    tabSelected(info : {selectedTab: Tab}){
        this.tabActivated.emit(info.selectedTab.tabId);
    }

    activateFriend(user: User) {
        if (!this.chatGlobals.isCurrentFriend(user)){
            this.chatGlobals.currentFriend = user;
            this.ownerActivated.emit();
        }
    }

    activateChat(chat: Chat) {
        if(!this.chatGlobals.isCurrentChat(chat)) {
            this.chatGlobals.currentChat = chat;
            this.ownerActivated.emit();
        }
    }

    friendIsActive(user : User) : boolean {
        return this.chatGlobals.isCurrentFriend(user);
    }

    friendIsOnline(user : User) : boolean {
        return user.online;
    }

    chatIsActive(chat : Chat) : boolean {
        return this.chatGlobals.isCurrentChat(chat);
    }
}