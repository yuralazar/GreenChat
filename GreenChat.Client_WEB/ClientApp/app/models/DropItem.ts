import {ContainerContent} from "angular2-modal";

export class DropItem{
    name : string;
    component: ContainerContent;
    inputs : any;

    constructor(name: string, component: ContainerContent, inputs: any) {
        this.name = name;
        this.component = component;
        this.inputs = inputs;
    }
}
