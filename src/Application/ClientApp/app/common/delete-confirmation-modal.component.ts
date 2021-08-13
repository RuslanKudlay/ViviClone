import { Component } from '@angular/core';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';

@Component({
    selector: 'delete-confirmation',
    templateUrl: 'delete-confirmation-modal.component.html'
})

export class DeleteConfirmationModalComponent extends DialogComponent<void, boolean> {
    constructor(dialogService: DialogService) {
        super(dialogService);
    }

    confirm() {
        this.result = true;
        this.close();
    }

    reset() {
        this.result = false;
        this.close();
    }
}
