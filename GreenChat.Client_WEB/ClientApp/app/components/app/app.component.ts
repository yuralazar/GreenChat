import {Component, ViewContainerRef} from '@angular/core';
import {BSModalContext, Modal} from "angular2-modal/plugins/bootstrap";
import {CustomModal} from "../modals/sample/sample";
import {overlayConfigFactory} from "angular2-modal";


@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent{

    constructor() {
    }
}
