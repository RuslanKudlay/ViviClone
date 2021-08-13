import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { IAfterGuiAttachedParams } from 'ag-grid-community';

@Component({
    selector: 'delite-button',
    template: `<a class='btn btn-xs btn-danger' *ngIf='id'>Del</a>`
})

export class DeleteButtonComponent implements ICellRendererAngularComp {
    public params: any;
    public subUrl: string;
    public id: number;

    refresh(params: any): boolean {
        throw new Error('Method not implemented.');
    }

afterGuiAttached ? (params?: IAfterGuiAttachedParams): void {
        throw new Error('Method not implemented.');
    }

    agInit(params: any): void {
        this.params = params;
        if (params.data) {
            this.id = params.data.id;
        }
    }
}
