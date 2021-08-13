import { Component } from '@angular/core';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';

export interface DialogModel {
    errMsg: string;
}

@Component({
    templateUrl: './error-modal.component.html'
})
export class ErrorModalComponent extends DialogComponent<DialogModel, string> implements DialogModel {
    title: string = 'Error';
    errMsg: string;

    constructor(dialogService: DialogService) {
        super(dialogService);
    }
}
