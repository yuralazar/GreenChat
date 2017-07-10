import { Component } from '@angular/core';
import {CloseGuard, DialogRef, ModalComponent} from 'angular2-modal';
import { BSModalContext } from 'angular2-modal/plugins/bootstrap';

export class CustomModalContext extends BSModalContext {
    public num1: number;
    public num2: number;
}
/**
 * A Sample of how simple it is to create a new window, with its own injects.
 */
@Component({
    selector: 'modal-content',
    templateUrl: "./sample.html",
    styleUrls: ["./sample.css"],
})
export class CustomModal implements CloseGuard, ModalComponent<CustomModalContext> {
    context: CustomModalContext;

    public wrongAnswer: boolean;

    constructor(public dialog: DialogRef<CustomModalContext>) {
        this.context = dialog.context;
        this.wrongAnswer = true;
        dialog.setCloseGuard(this);
    }

    onKeyUp(value) {
        this.wrongAnswer = value != 5;
        this.dialog.close();
    }


    beforeDismiss(): boolean {
        return true;
    }

    beforeClose(): boolean {
        return this.wrongAnswer;
    }
}
