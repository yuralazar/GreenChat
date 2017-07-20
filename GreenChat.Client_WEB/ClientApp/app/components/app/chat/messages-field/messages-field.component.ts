import {
    AfterViewChecked, Component, ComponentFactoryResolver, ElementRef, EventEmitter, Output, ReflectiveInjector,
    ViewChild,
    ViewContainerRef
} from '@angular/core';
import { User } from "../../../../models/User";
import {ChatMessagesService} from "../../../../services/chat-messages.service";
import {PrivateMessagesService} from "../../../../services/private-messages.service";
import {FriendsService} from "../../../../services/friends.service";
import {MessageItemComponent} from "../message-item/message-item.component";
import {ChatGlobalsService} from "../../../../services/chat-globals.service";
import {Message} from "../../../../models/Message";
import {Chat} from "../../../../models/Chat";
import {MessageContent} from "../../../../models/MessageContent";

@Component({
    selector: 'messages-field',
    templateUrl: './messages-field.component.html',
    styleUrls: ['./messages-field.component.css']
})
export class MessagesFieldComponent implements AfterViewChecked{

    componentData = null;
    messages : Message[] = [];
    @ViewChild('messageList') private messageList: ElementRef;
    @Output() scrolledToTop = new EventEmitter<boolean>();
    needToScrollDown : boolean = false;
    showBusy : boolean = false;

    constructor(private viewContainerRef: ViewContainerRef
        ,private chatGlobals: ChatGlobalsService
        ,private componentFactoryResolver: ComponentFactoryResolver
        ,private friendsService : FriendsService
        ,private privateMessagesService : PrivateMessagesService
        ,private chatMessagesService : ChatMessagesService){}

    createAndAppendPrivateMessage(userFrom: any, message : any) {
        let user = this.friendsService.getFriendById(userFrom.id);
        this.friendsService.changeOnlineStatus(userFrom);
        let mess = this.privateMessagesService.createMessage(user, this.chatGlobals.currentUser, message
                                                            , !this.chatGlobals.isCurrentFriend(userFrom), true);
    }

    createAndAppendChatMessage(chat: Chat, userFrom: any, message : any) {
        let user = this.friendsService.getFriendById(userFrom.id);
        this.friendsService.changeOnlineStatus(userFrom);
        let mess = this.chatMessagesService.createMessage(chat, userFrom, message
                                                            , !this.chatGlobals.isCurrentChat(chat), true);
    }

    refillMessages() {
        this.messages = [];

        if (this.chatGlobals.privateMode())
        {
            this.messages = this.privateMessagesService.getMessages(this.chatGlobals.currentFriend);
        }
        else {
            this.messages = this.chatMessagesService.getMessages(this.chatGlobals.currentChat);
        }
    }

    ngAfterViewChecked(): void {
        if(this.needToScrollDown){
            this.scrollToBottom();
            if (this.messages.length > 0)
                this.needToScrollDown = false;
        }
    }

    scrollToBottom(): void {
        try {
            this.messageList.nativeElement.scrollTop = this.messageList.nativeElement.scrollHeight;
        } catch(err) { }
    }

    onScroll(){
        if (this.messageList.nativeElement.scrollTop === 0){
            this.scrolledToTop.emit(true);
        }
    }

    moveScroll(count: number) {
        if (count > 0)
            try {
                this.messageList.nativeElement.scrollTop = 10*count;
            } catch(err) {
                this.scrollToBottom();
            }
    }

    needToShowDate(index : number) : boolean{
        if (index == 0)
            return true;
        else {
            let date = this.messages[index].content.date.getDay();
            let prevDate = this.messages[index-1].content.date.getDay();
            if (date != prevDate)
                return true;
        }

        return false;
    }
}