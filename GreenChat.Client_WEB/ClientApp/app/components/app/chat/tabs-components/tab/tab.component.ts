import { Component, Input, OnInit } from '@angular/core';
import {Tab} from "../../../../../interfaces/tab.interface";
import {TabsComponent} from "../tabs/tabs.component";
import {DropItem} from "../../../../../models/DropItem";

@Component({
    selector: 'my-tab',
    templateUrl: './tab.component.html'
})
export class TabComponent implements OnInit, Tab {
    selected: boolean;
    @Input() animate: boolean;
    @Input() countNewMessages : number;
    @Input() tabTitle;
    @Input() tabId;

    @Input() menuItems : DropItem[] = [];

    constructor(private tabsComponent: TabsComponent) {}
    ngOnInit() {
        this.tabsComponent.addTab(this);
    }
}