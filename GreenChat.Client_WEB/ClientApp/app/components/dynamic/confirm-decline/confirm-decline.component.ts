import {Component, EventEmitter, Output} from '@angular/core';

@Component({
    selector: 'confirm-decline',
    templateUrl: './confirm-decline.component.html',
    styleUrls: ['./confirm-decline.component.css']
})
export default class ConfirmDeclineComponent {

    confirm : string = "Confirm";
    decline : string = "Decline";

    @Output() confirmed = new EventEmitter<boolean>();

    onConfirm(){
        this.confirmed.emit(true);
    }

    onDecline(){
        this.confirmed.emit(false);
    }

}