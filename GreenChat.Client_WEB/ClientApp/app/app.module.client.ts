import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { ModalModule } from 'angular2-modal';
import { BootstrapModalModule } from 'angular2-modal/plugins/bootstrap';
import { sharedConfig } from './app.module.shared';
import {FriendsService} from "./services/friends.service";
import {ChatsService} from "./services/chats.service";
import {ChatMessagesService} from "./services/chat-messages.service";
import {PrivateMessagesService} from "./services/private-messages.service";
import {ChatService} from "./services/chat.service";
import {WebsocketService} from "./services/websocket.service";
import {MessageItemComponent} from "./components/app/chat/message-item/message-item.component";
import {FriendsAndChatsItemComponent} from "./components/app/chat/friends-chat-item/friends-chats-item.component";
import {CustomModal} from "./components/modals/sample/sample";
import {SearchFriendModal} from "./components/modals/search-friend/search-friend";
import {WsMessageService} from "./services/wsMessage.service";
import {ModalGenerator} from "./components/modals/modal-generator";
import {ChatGlobalsService} from "./services/chat-globals.service";
import {CreateChatModal} from "./components/modals/create-chat/create-chat";
import {SelectChatModal} from "./components/modals/select-chat/select-chat";
import {SelectFriendsModal} from "./components/modals/select-friends/select-friends";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {BusyModule} from "angular2-busy";

@NgModule({
    bootstrap: sharedConfig.bootstrap,
    declarations: sharedConfig.declarations,
    entryComponents : [
            MessageItemComponent,
            FriendsAndChatsItemComponent,
            CustomModal,
            SearchFriendModal,
            CreateChatModal,
            SelectChatModal,
            SelectFriendsModal
        ],
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        ModalModule.forRoot(),
        BootstrapModalModule,
        BrowserAnimationsModule,
        BusyModule,
        ...sharedConfig.imports
    ],
    providers: [
        ChatService,
        WebsocketService,
        PrivateMessagesService,
        ChatMessagesService,
        FriendsService,
        ChatsService,
        WsMessageService,
        ModalGenerator,
        ChatGlobalsService,
        { provide: 'ORIGIN_URL', useValue: location.origin }
    ]
})
export class AppModule {
}
