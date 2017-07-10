import { Injectable } from '@angular/core';
import {FriendsContainer} from "../models/FriendsContainer";
import {User} from "../models/User";

@Injectable()
export class FriendsService{

    container : FriendsContainer = new FriendsContainer();

    createFriend(id: string, email: string, online: boolean, potential: boolean) : User{
        let friend = new User(id, email, online, potential);
        this.addFriend(friend);
        return friend;
    }

    changeOnlineStatus(user : User){
        let friend = this.getFriendById(user.id);
        if (friend !== undefined)
            friend.online = user.online;
    }

    addFriend(friend: User) {
        this.container.addFiend(friend);
    }

    getFriendById(id : string) : User {
        return this.container.getFriends().find(user => user.id === id);
    }

    getFriendByEmail(email: string) : User {
        return this.container.getFriends().find(user => user.email === email);
    }

    getFriends() : User[] {
        return this.container.getFriends();
    }
}
