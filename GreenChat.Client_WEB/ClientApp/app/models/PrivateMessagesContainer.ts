import {MessagesContainer} from "./MessagesContainer";
import {User} from "./User";

export class PrivateMessagesContainer extends MessagesContainer {

    constructor(user: User) {
        super(user);
    }
}

