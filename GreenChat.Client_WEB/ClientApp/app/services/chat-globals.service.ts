import {User} from "../models/User";
import {Chat} from "../models/Chat";
import {PrivateMessagesService} from "./private-messages.service";
import {ChatMessagesService} from "./chat-messages.service";
import {MessagesService} from "./message-service";
export class ChatGlobalsService{

    // 0 - private , 1 - chat
    private _currentMode : number = 0;
    private _currentUser : User;
    private _currentFriend : User;
    private _currentChat : Chat;

    getReceiver() : User | Chat{
        if (this.privateMode()){
            return this.currentFriend;
        } else{
            return this.currentChat;
        }
    }

    setPrivateMode() {
        this.currentMode = 0;
        this.currentChat = null;
    }

    setChatMode() {
        this.currentMode = 1;
        this.currentFriend = null;
    }

    privateMode() :boolean{
        return this.currentMode === 0;
    }

    getCurrentOwner() : User|Chat{
        return this.privateMode() ? this.currentFriend : this.currentChat;
    }

    getCurrentMessageService(privateMessagesService : PrivateMessagesService, chatMessagesService : ChatMessagesService) : MessagesService{
        return this.privateMode() ? privateMessagesService : chatMessagesService;
    }

    isCurrentFriend(userFrom: User) {
        if (this.currentFriend === null || this.currentFriend === undefined)
            return false;
        return this.currentFriend.id === userFrom.id;
    }

    isCurrentChat(chat: Chat) {
        if (this.currentChat=== null || this.currentChat === undefined)
            return false;
        return this.currentChat.id === chat.id;
    }

    isCurrentUser(userFrom: any|User) :boolean {
        if (this.currentUser === null || this.currentUser === undefined)
            return false;
        return this.currentUser.id === userFrom.id;
    }

    get currentMode(): number {
        return this._currentMode;
    }

    set currentMode(value: number) {
        this._currentMode = value;
    }

    get currentUser(): User {
        return this._currentUser;
    }

    set currentUser(value: User) {
        this._currentUser = value;
    }

    get currentFriend(): User {
        return this._currentFriend;
    }

    set currentFriend(value: User) {
        this._currentFriend = value;
    }

    get currentChat(): Chat {
        return this._currentChat;
    }

    set currentChat(value: Chat) {
        this._currentChat = value;
    }
}