import {Modal} from "angular2-modal/plugins/bootstrap";
import {Injectable} from "@angular/core";

@Injectable()
export class ModalGenerator{

    public showAlert(modal: Modal, title : string, body : string, size : 'sm' | 'lg' = "sm"){
        return modal.alert()
            .size(size)
            .showClose(true)
            .title(title)
            .body(body)
            .okBtnClass("btn btn-success")
            .open();
    }

    public showConfirm(modal: Modal, title : string, body : string, size : 'sm' | 'lg' = "sm"){
        return modal.confirm()
            .size(size)
            .showClose(true)
            .title(title)
            .body(body)
            .okBtnClass("btn btn-success")
            .open()
    }
}