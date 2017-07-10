import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Rx';
import { WebsocketService } from './websocket.service';

@Injectable()
export class ChatService{

    messageData: Subject<string>;
    server: string = window.location.hostname;
    port: string = "5000";
    secure: boolean = false;
    route: string = "ws";

    constructor(private wsService: WebsocketService) {
        let url = (this.secure ? "wss" : "ws") + "://" + this.server + ":" + this.port + "/" + this.route;
        this.messageData = <Subject<string>>wsService
            .connect(url)
            .map((response: MessageEvent): string => {
                return response.data;
            });
    }
}
