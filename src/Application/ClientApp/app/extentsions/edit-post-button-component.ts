﻿import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { IAfterGuiAttachedParams } from 'ag-grid-community';

@Component({
    selector: 'edit-post-button',
    template: `<a class="btn btn-xs btn-primary" *ngIf="subUrl" routerLink="/posts/{{subUrl}}">Edit</a>`
})

export class EditPostButtonComponent implements ICellRendererAngularComp {
    public params: any;
    public subUrl: string;
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
            this.subUrl = params.data.subUrl;
            this.id = params.data.id;
        }
    }
}