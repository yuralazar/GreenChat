import {MessagesContainer} from "../models/MessagesContainer";
import {Chat} from "./Chat";

export class ChatMessagesContainer extends MessagesContainer {

    constructor(chat: Chat) {
        super(chat);
    }
}

