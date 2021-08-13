import { Component } from '@angular/core';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';

@Component({
    selector: 'iconModal',
    templateUrl: 'icon-modal.component.html'
})

export class IconModalComponent extends DialogComponent<void, string> {
    listOfIconNames: Array<string> = ['flaticon-24-hours-support', 'flaticon-arrows', 'flaticon-atom', 'flaticon-bars', 'flaticon-check-square', 'flaticon-checked', 'flaticon-commerce',
                                    'flaticon-diamond', 'flaticon-dollar-symbol', 'flaticon-four-grid-layout-design-interface-symbol', 'flaticon-home', 'flaticon-leather-derby-shoe',
                                    'flaticon-like', 'flaticon-monitor', 'flaticon-multiple-computers-connected', 'flaticon-photo-camera', 'flaticon-return-of-investment', 'flaticon-right-arrow-circular-button',
                                    'flaticon-search', 'flaticon-shield', 'flaticon-soccer-ball', 'flaticon-squares-gallery-grid-layout-interface-symbol', 'flaticon-table-lamp', 'flaticon-technology',
                                    'flaticon-time', 'flaticon-timer', 'flaticon-transport', 'flaticon-umbrella', 'flaticon-user-outline'];
    chosenIcon: string = '';
    isError: boolean = false;

    constructor(dialogService: DialogService) {
        super(dialogService);
    }

    confirm() {
        if (this.chosenIcon) {
            this.result = this.chosenIcon;
            this.close();
        } else {
            this.isError = true;
        }
    }

    changeIcon(item) {
        this.isError = false;
        this.chosenIcon = item;
    }
}
