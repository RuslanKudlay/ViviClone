import { Component, OnInit } from '@angular/core';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';
import { CategoryValues } from '../../models/category-values.model';

export interface DialogModel {
    categoryValues: Array<any>;
}
@Component({
    selector: 'categoryValuesSelectModal',
    templateUrl: 'category-values-select-modal.component.html',
    styleUrls: ['./category-values-select-modal.component.css']
})
export class CategoryValuesSelectModalComponent extends DialogComponent<DialogModel, any> implements DialogModel {
    public categoryValues: Array<any> = new Array();

    constructor(dialogService: DialogService) { 
        super(dialogService);
    }

    public confirm(): void {
        this.result = this.categoryValues.filter(x => x.isChecked == true);
        this.close();
    }

    public select(event, item): void {
        if (this.categoryValues.length > 1) {
            let categoryValuesChecked = this.categoryValues.filter(x => x.isChecked == true);
            if (categoryValuesChecked && categoryValuesChecked.length > 0) {
                categoryValuesChecked[0].isChecked = false;
            }
        }
        item.isChecked =  !item.isChecked;
    }
}
