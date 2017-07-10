import {User} from "./User";
export class Chat {
    id : number;
    name : string;
    users: User[] = [];

    constructor(id: number, name: string, users : User[]) {
        this.id = id;
        this.name = name;
        this.users = users;
    }
}