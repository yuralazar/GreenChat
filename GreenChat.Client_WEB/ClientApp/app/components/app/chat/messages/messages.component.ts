import {Component, ComponentFactoryResolver, ReflectiveInjector, ViewChild, ViewContainerRef} from '@angular/core';
import {ChatMessagesService} from "../../../../services/chat-messages.service";
import {PrivateMessagesService} from "../../../../services/private-messages.service";
import {FriendsService} from "../../../../services/friends.service";

@Component({
    selector: 'messages',
    templateUrl: './messages.component.html',
    styleUrls: ['./messages.component.css']
})
export class MessagesComponent {

    constructor(private viewContainerRef: ViewContainerRef
        ,private componentFactoryResolver: ComponentFactoryResolver
        ,private friendsService : FriendsService
        ,private privateMessagesService : PrivateMessagesService
        ,private chatMessagesService : ChatMessagesService){}

    appendMessage(data: {component: any, inputs: any }){
        // Inputs need to be in the following format to be resolved properly
        let inputProviders = Object.keys(data.inputs).map((inputName) => {return {provide: inputName, useValue: data.inputs[inputName]};});
        let resolvedInputs = ReflectiveInjector.resolve(inputProviders);
        // We create an injector out of the data we want to pass down and this components injector
        let injector = ReflectiveInjector.fromResolvedProviders(resolvedInputs);
        // We create a factory out of the component we want to create
        let factory = this.componentFactoryResolver.resolveComponentFactory(data.component);
        // We create the component using the factory and the injector
        const ref = this.viewContainerRef.createComponent(factory, null, injector);
        ref.changeDetectorRef.detectChanges();
    }
}