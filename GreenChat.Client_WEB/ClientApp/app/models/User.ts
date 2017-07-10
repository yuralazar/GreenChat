export class User {
    id: string;
    email: string;
    online: boolean;
    potential: boolean;

    constructor(id: string, email: string, online: boolean = false, potential: boolean = false) {
        this.id = id;
        this.email = email;
        this.online = online;
        this.potential = potential;
    }

    static create(user: {id : string, email: string, online : boolean, potential:boolean}) : User {
        return new User(user.id, user.email, user.online, user.potential);
    }
}