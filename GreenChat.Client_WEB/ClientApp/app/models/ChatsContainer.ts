import {Chat} from "./Chat";

export class ChatsContainer {

    private chats : Chat[] = [];

    getChats() : Chat[] {
        return this.chats;
    }

    addChat(chat: Chat) {
        this.chats.push(chat);
    }
}
