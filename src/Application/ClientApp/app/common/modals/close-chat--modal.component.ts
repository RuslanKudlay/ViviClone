import { Component, Input, AfterViewInit } from '@angular/core';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';

@Component({
    selector: 'close-chat',
    templateUrl: 'close-chat-modal.component.html'
})
export class CloseChatModalComponent extends DialogComponent<void, boolean> {
    constructor(dialogService: DialogService) {
        super(dialogService);
    }

    confirm() {
        this.result = true;
        this.close();
    }
}
