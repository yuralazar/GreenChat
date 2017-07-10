import { Injectable } from '@angular/core';
import * as Rx from 'rxjs/Rx';

@Injectable()
export class WebsocketService {

    constructor() {}

    private subject: Rx.Subject<MessageEvent>;

    public connect(url : string): Rx.Subject<MessageEvent> {
        if (!this.subject) {           
            this.subject = this.create(url);
            console.log("Successfully connected: " + url);
        }
        return this.subject;
    }

    private create(url): Rx.Subject<MessageEvent> {
        let websocket = new WebSocket(url);

        let observable = Rx.Observable.create(
            (obs: Rx.Observer<MessageEvent>) => {
                websocket.onmessage = obs.next.bind(obs);
                websocket.onerror = obs.error.bind(obs);
                websocket.onclose = obs.complete.bind(obs);
                return websocket.close.bind(websocket);
            })

        let observer = {
            next: (data: Object) => {
                if (websocket.readyState === WebSocket.OPEN) {
                    websocket.send(data);
                }
            }
        }

        return Rx.Subject.create(observer, observable);
    }

}
