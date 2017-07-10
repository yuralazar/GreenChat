import {Component, Injectable} from '@angular/core';

@Injectable()
export class DynamicComponentsService{

    components : any[] = [];

    addComponent(component : any){
        this.components.push(component);
    }

    clear(){
        this.components.forEach(comp => {
            comp.destroy()
        });

        this.components = [];
    }
}
