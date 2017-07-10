import { NgModule } from '@angular/core';
import { AppComponent } from './components/app/app.component'
import { ChatComponent } from './components/app/chat/chat.component'
import {FriendsAndChatsComponent} from "./components/app/chat/friends-chat-list/friends-chats-list.component";
import {MessagesFieldComponent} from "./components/app/chat/messages-field/messages-field.component";
import {MessageInputComponent} from "./components/app/chat/message-input/message-input.component";
import {MessageItemComponent} from "./components/app/chat/message-item/message-item.component";
import ChatEditComponent from "./components/dynamic/chat-edit/chat-edit.component";
import ConfirmDeclineComponent from "./components/dynamic/confirm-decline/confirm-decline.component";
import ChatRequestComponent from "./components/dynamic/chat-request/chat-request.component";
import FriendRequestComponent from "./components/dynamic/friend-request/frient-request.component";
import DynamicComponent from "./components/dynamic/dynamic-component";
import {FriendsAndChatsItemComponent} from "./components/app/chat/friends-chat-item/friends-chats-item.component";
import {TabComponent} from "./components/app/chat/tabs-components/tab/tab.component";
import {TabsComponent} from "./components/app/chat/tabs-components/tabs/tabs.component";
import {DropDownMenuComponent} from "./components/app/chat/drop-down-menu/drop-down-menu.component";
import {CustomModal} from "./components/modals/sample/sample";
import {SearchFriendModal} from "./components/modals/search-friend/search-friend";
import {CreateChatModal} from "./components/modals/create-chat/create-chat";
import {SelectChatModal} from "./components/modals/select-chat/select-chat";
import {SelectFriendsModal} from "./components/modals/select-friends/select-friends";
import {BusyComponent} from "./components/app/chat/busy/busy.component";
import {DatexPipe} from "./pipes/datex";
import { AvatarComponent } from 'ng2-avatar';

export const sharedConfig: NgModule = {
    bootstrap: [ AppComponent ],
    declarations: [
        AppComponent,
        ChatComponent,
        FriendsAndChatsComponent,
        MessagesFieldComponent,
        MessageInputComponent,
        MessageItemComponent,
        ChatEditComponent,
        ConfirmDeclineComponent,
        ChatRequestComponent,
        FriendRequestComponent,
        DynamicComponent,
        FriendsAndChatsItemComponent,
        TabComponent,
        TabsComponent,
        DropDownMenuComponent,
        CustomModal,
        SearchFriendModal,
        CreateChatModal,
        SelectChatModal,
        SelectFriendsModal,
        BusyComponent,
        DatexPipe,
        AvatarComponent
    ],
    imports: [
        /*RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent },
            { path: '**', redirectTo: 'home' }
        ])
        */
    ]
};
