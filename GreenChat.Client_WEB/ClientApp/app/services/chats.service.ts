import { Injectable } from '@angular/core';
import {Chat} from "../models/Chat";
import {ChatsContainer} from "../models/ChatsContainer";
import {User} from "../models/User";

@Injectable()
export class ChatsService{

    container : ChatsContainer = new ChatsContainer();

    createChat(id: number, name: string, users: User[]) : Chat{
        let chat = new Chat(id, name, users);
        this.addChat(chat);
        return chat;
    }

    private addChat(chat : Chat) {
        this.container.addChat(chat);
    }

    getChatById(id : number) : Chat {
        return this.container.getChats().find(chat => chat.id === id);
    }

    getChats() : Chat[] {
        return this.container.getChats();
    }
}
