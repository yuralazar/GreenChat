import {Component, Injector, Input, ViewChild, ViewContainerRef} from '@angular/core';
import {DropItem} from "../../../../models/DropItem";
import {BSModalContext, Modal} from "angular2-modal/plugins/bootstrap";
import {Overlay, overlayConfigFactory} from "angular2-modal";
import {SearchFriendModal} from "../../../modals/search-friend/search-friend";
import {User} from "../../../../models/User";
import {subscribeToResult} from "rxjs/util/subscribeToResult";
import {Chat} from "../../../../models/Chat";
import {Observable} from "rxjs/Observable";

@Component({
    selector: 'drop-down-menu',
    templateUrl: './drop-down-menu.component.html',
    styleUrls: ['./drop-down-menu.component.css']
})
export class DropDownMenuComponent {

    showDropDown : boolean = false;
    @Input() menuItems : DropItem[];

    constructor(public modal: Modal) {
        console.log();
    }

    openModal(item : DropItem) {
        this.showDropDown = false;
        this.modal
            .open(item.component, overlayConfigFactory(item.inputs, BSModalContext));
            //.then(value => value.result.then(result => this.transportResult(result)));
    }
}