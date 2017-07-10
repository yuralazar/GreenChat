import {User} from "./User";

export class FriendsContainer {

    private friends : User[] = [];

    getFriends() : User[] {
        return this.friends;
    }

    addFiend(friend: User) {
        this.friends.push(friend);
    }
}
