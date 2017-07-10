import {AfterViewChecked, Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import { ChatService } from "../../../services/chat.service";
import { FriendsAndChatsComponent } from "./friends-chat-list/friends-chats-list.component";
import { User } from "../../../models/User";
import { Chat } from "../../../models/Chat";
import {MessagesFieldComponent} from "./messages-field/messages-field.component";
import {PrivateMessagesService} from "../../../services/private-messages.service";
import {ChatMessagesService} from "../../../services/chat-messages.service";
import {WsMessageService} from "../../../services/wsMessage.service";
import {Modal, TwoButtonPreset} from "angular2-modal/plugins/bootstrap";
import {ModalGenerator} from "../../modals/modal-generator";
import {DialogRef} from "angular2-modal";
import {ChatGlobalsService} from "../../../services/chat-globals.service";
import {MessagesService} from "../../../services/message-service";
import {FriendsService} from "../../../services/friends.service";
import {Subscription} from "rxjs/Subscription";

@Component({
    selector: 'chat',
    templateUrl: './chat.component.html',
    styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit{

    enableLogging = true;
    loadNumber : number = 25;

    @ViewChild(FriendsAndChatsComponent)
    private friendsAndChatsComponent: FriendsAndChatsComponent;
    @ViewChild(MessagesFieldComponent)
    private messagesFieldComponent: MessagesFieldComponent;

    constructor(
         private chatGlobals: ChatGlobalsService
        , private chatService: ChatService
        , private friendsService : FriendsService
        , private privateMessagesService : PrivateMessagesService
        , private wsMessageService : WsMessageService
        , private chatMessagesService : ChatMessagesService
        , private modalGenerator : ModalGenerator
        , vcRef: ViewContainerRef, public modal: Modal)
    {
        chatService.messageData.subscribe(msg => this.webSocketMessageHandler(msg));
        modal.overlay.defaultViewContainer = vcRef;
    }

    ngOnInit(): void {
        this.friendsAndChatsComponent.showBusy = true;
    }

    tabActivated(tabId : string){
        if (tabId === "0"){
            this.chatGlobals.setPrivateMode();
        }
        else {
            this.chatGlobals.setChatMode();
        }
    }

    ownerActivated(){
        this.manageUnreadMessages();
        this.getHistory();
        this.messagesFieldComponent.needToScrollDown = true;
    }

    private webSocketMessageHandler(data: string): void {
        let message = JSON.parse(data);
        if (this.enableLogging)
            console.log(message.type + " " + message.arguments);
        let type = message.type;
        this.handleMessageByType(type, message.arguments);
    };

    private handleMessageByType(type: number, args : any) {
        let messageArgs = JSON.parse(args);
        switch (type) {
            case 0:
                if (this.enableLogging) console.log('Server Error :' + args);
                break;
            case 1: this.userConnectionState(messageArgs); break;
            case 2: this.privateMessage(messageArgs); break;
            case 3: this.chatMessage(messageArgs); break;
            case 4: this.userFound(messageArgs); break;
            case 5: this.friendRequest(messageArgs); break;
            case 6: this.friendConfirmed(messageArgs); break;
            case 7: this.chatCreated(messageArgs); break;
            case 8: this.chatRequest(messageArgs); break;
            case 9: this.chatConfirmed(messageArgs); break;
            case 10: this.initialInfo(messageArgs); break;
            case 11: this.privateMessages(messageArgs); break;
            case 12: this.chatMessages(messageArgs); break;
            default:
        };
    }
    setCurrentUser(id: string, email: string) {
        this.chatGlobals.currentUser = User.create({id : id, email : email, online : true, potential: false});
    }

    private initialInfo(data: any) {
        this.setCurrentUser(data.user.id, data.user.email);
        this.friendsAndChatsComponent.fillFriendsList(data.friends);
        this.friendsAndChatsComponent.fillChatsList(data.chatRooms, data.chatsUsers);
        this.friendsAndChatsComponent.showBusy = false;
        this.showFriendRequests(data.friends);
        this.showChatRequests(data.chatRequests);
        this.addUnreadMessages(data);
    }

    private userFound(data: any) {
        if (data.user.id === null)
            this.modalGenerator.showAlert(this.modal, "Searching friend", "User not found!");
        else
            this.modalGenerator.showConfirm(this.modal, "Searching friend",
            "<h4>" + "User " + "<span class=\"green-text\">" + data.user.email + "</span> was found. You can add him to friends list" + "</h4>")
            .then((dialog: DialogRef<TwoButtonPreset>) => {
                dialog.result.then(result => this.addFriend(data.user))
            });
    }

    private friendConfirmed(data: any) {
        let infoMessage = "";
        if (data.confirmed) {
            this.friendsAndChatsComponent.createAndAppendFriend(data.userFrom);
            infoMessage = "<span class=\"green-text\">" + data.userFrom.email + "</span> is now yor friend";
        }else {
            infoMessage = "<span class=\"green-text\">" + data.userFrom.email + "</span> declined friendship request";
        }
        this.modalGenerator.showAlert(this.modal, "Friendship request", infoMessage);
    }

    private friendRequest(data: any) {
        this.showFriendRequest(data.userFrom);
    }

    private privateMessage(data: any) {
        this.messagesFieldComponent.createAndAppendPrivateMessage(data.userFrom, data.message);
        if (this.chatGlobals.isCurrentFriend(data.userFrom)){
            this.messagesFieldComponent.needToScrollDown = true;
        }
    }

    private chatMessage(data: any) {
        this.messagesFieldComponent.createAndAppendChatMessage(data.chat, data.userFrom, data.message);
        if (this.chatGlobals.isCurrentChat(data.chat)){
            this.messagesFieldComponent.needToScrollDown = true;
        }
    }

    private privateMessages(data: any) {
        this.loadMessages(this.privateMessagesService, data.userFrom, data, true);
    }

    private chatMessages(data: any) {
        this.loadMessages(this.chatMessagesService, data.chat, data, true);
    }

    private showFriendRequests(friends: User[]) {
        friends.forEach(fr=> {
            if (fr.potential){
                this.showFriendRequest(fr);
            }
        })
    }

    private showFriendRequest(userFrom: any|User) {
        this.modalGenerator.showConfirm(this.modal, "Friendship request",
            "<h4>" + "User " + "<span class=\"green-text\">" + userFrom.email + "</span> wants to become your friend" + "</h4>")
            .then((dialog: DialogRef<TwoButtonPreset>) => {
                dialog.result.then(result => this.friendshipConfirmation(userFrom, true))
                    .catch(() => this.friendshipConfirmation(userFrom, false))
            });
    }

    private chatCreated(data: any) {
        this.modalGenerator.showAlert(this.modal, "Chat creation", "Chat " +
            "<span class=\"green-text\">" + data.chat.name + "</span> was created. You can add friends to it.");
        this.friendsAndChatsComponent.createAndAppendChat(data.chat, [this.chatGlobals.currentUser]);
    }

    private chatRequest(data: any) {
        this.modalGenerator.showConfirm(this.modal, "Chat room invitation",
            "<h4>" + "User "  + "<span class=\"green-text\">" + data.userFrom.email + "</span> invites you to chat room " + data.chat.name + "</h4>")
            .then((dialog: DialogRef<TwoButtonPreset>) => {
                dialog.result.then(result => this.chatRoomConfirmation(data.chat, data.userFrom, data.chatUsers, true))
                    .catch(() => this.chatRoomConfirmation(data.chat, data.userFrom, data.chatUsers, false))
            });
    }

    private friendshipConfirmation(userFrom: User, confirmed: boolean) {
        this.wsMessageService.addOrConfirmFriend(userFrom, false, confirmed);
        if (confirmed){
            this.friendsAndChatsComponent.createAndAppendFriend(userFrom);
        }
    }

    private chatRoomConfirmation(chat: Chat|any, userFrom : User, chatUsers : User[],  confirmed: boolean) {
        this.wsMessageService.confirmChatRequest(chat, userFrom, confirmed);
        if (confirmed){
            this.friendsAndChatsComponent.createAndAppendChat(chat, chatUsers);
        }
    }

    private chatConfirmed(data: any) {
        let infoMessage = "";
        if (data.confirmed) {
            infoMessage = "<span class=\"green-text\">" + data.userFrom.email + "</span> joined chat room '" + data.chat.name + "'";
        }else {
            infoMessage = "<span class=\"green-text\">" + data.userFrom.email + "</span> refused to join chat room '" + data.chat.name + "'";
        }
        this.modalGenerator.showAlert(this.modal, "Chat request", infoMessage);
    }

    private showChatRequests(chatRequests: any) {
        chatRequests.forEach(data => this.chatRequest(data));
    }

    getOldMessages(){
        this.getHistory(true);
    }

    getHistory(scrolledHistory : boolean = false){
        let currentOwner = this.chatGlobals.getCurrentOwner();
        let service = this.currentMessageService();
        let messageContainer = service.getContainerByOwner(currentOwner);
        if (!messageContainer.historyLoaded){
            this.messagesFieldComponent.showBusy = true;
            service.getHistory(currentOwner, this.loadNumber);
        }
        else{
            if (scrolledHistory)
                service.getHistory(currentOwner, this.loadNumber, messageContainer.getStartDate());
            else
                this.messagesFieldComponent.refillMessages();
        }
    }

    private manageUnreadMessages() {
        let currentOwner = this.chatGlobals.getCurrentOwner();
        let service = this.currentMessageService();
        let messageContainer = service.getContainerByOwner(currentOwner);
        if (messageContainer.countNew() > 0){
            messageContainer.unmarkNew();
            if (!messageContainer.historyLoaded) {
                service.deleteUnreadMessages(currentOwner);
            }
        }
    }

    private currentMessageService(){
        return this.chatGlobals.getCurrentMessageService(this.privateMessagesService, this.chatMessagesService);
    }

    private addUnreadMessages(data: any) {
        data.privateMessages.forEach(pr =>
            this.privateMessagesService.createMessage(pr.userFrom, pr.userTo, pr.message, true, true));

        data.chatMessages.forEach(ch =>
            this.chatMessagesService.createMessage(ch.chat, ch.userFrom, ch.message, true, true));
    }

    scrollMessageField(){
        this.messagesFieldComponent.needToScrollDown = true;
    }

    private loadMessages(service: MessagesService, owner:User|Chat, data : any, historyLoaded: boolean) {
        service.loadMessages(owner, data.messages, historyLoaded);
        this.messagesFieldComponent.refillMessages();
        this.messagesFieldComponent.moveScroll(data.messages.length);
        this.messagesFieldComponent.showBusy = false;
    }

    private userConnectionState(data: any) {
        this.friendsService.changeOnlineStatus(data);
    }

    private addFriend(user: User) {
        this.wsMessageService.addOrConfirmFriend(user, true, false);
    }
}
