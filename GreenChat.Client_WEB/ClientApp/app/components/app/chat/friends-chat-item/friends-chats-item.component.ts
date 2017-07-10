import {Component, Injector, ViewChild} from '@angular/core';

@Component({
    selector: 'friends-chats-item',
    templateUrl: './friends-chats-item.component.html',
    styleUrls: ['./friends-chats-item.component.css']
})
export class FriendsAndChatsItemComponent {

    text : string;

    constructor(private injector: Injector) {
        this.text = this.injector.get("text");
    }
}