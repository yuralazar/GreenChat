import { Component, EventEmitter, Output } from '@angular/core';
import {Tab} from "../../../../../interfaces/tab.interface";
import {DropItem} from "../../../../../models/DropItem";
import {PrivateMessagesService} from "../../../../../services/private-messages.service";
import {ChatMessagesService} from "../../../../../services/chat-messages.service";

@Component({
    selector: 'my-tabs',
    templateUrl: './tabs.component.html'
})
export class TabsComponent{

    tabs:Tab[] = [];
    showDropDown : boolean = false;
    dropItems : DropItem[] = [];
    @Output() selected = new EventEmitter();


    constructor(private privateMessagesService : PrivateMessagesService
               ,private chatMessagesService : ChatMessagesService) {
    }

    addTab(tab:Tab) {
        if (!this.tabs.length) {
            tab.selected = true;
        }
        this.tabs.push(tab);
    }

    selectTab(tab:Tab) {
        this.tabs.map((tab) => {
            tab.selected = false;
        })
        tab.selected = true;
        this.selected.emit({selectedTab: tab});
    }

    animateTab(id: string) {
        if (id === "0")
            return this.privateMessagesService.countNew() > 0;
        else if (id === "1")
            return this.chatMessagesService.countNew() > 0;
        return false;
    }
}