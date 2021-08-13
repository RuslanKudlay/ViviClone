import { Component } from '@angular/core';
import {ICellRendererAngularComp} from 'ag-grid-angular';
import { IAfterGuiAttachedParams } from 'ag-grid-community';

@Component({
    selector: 'edit-user-button',
    template: `<a class='btn btn-xs btn-primary' *ngIf='id' routerLink='/users/{{id}}'>Edit</a>`
})
export class EditUserButtonComponent implements ICellRendererAngularComp {
    public params: any;
    public id: number;

    refresh(params: any): boolean {
        throw new Error('Method not implemented.');
    }

    afterGuiAttached?(params?: IAfterGuiAttachedParams): void {
        throw new Error('Method not implemented.');
    }

    agInit(params: any): void {
        this.params = params;
        if (params.data) {
            this.id = params.data.id;
        }
    }

}
