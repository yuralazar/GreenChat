import {Component, Injector, Input} from '@angular/core';
import {ChatMessage, Message} from "../../../../models/Message";

@Component({
    selector: 'message-item',
    templateUrl: './message-item.component.html',
    styleUrls: ['./message-item.component.css']
})
export class MessageItemComponent {

    @Input() message : Message;

    isChatMessage(message : Message) : boolean{
        return message instanceof ChatMessage;
    }
}