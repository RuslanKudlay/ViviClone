import { AfterViewInit, Component } from '@angular/core';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';
import { CategoryValues } from '../../../models/category-values.model';

export interface DialogModel {
    name: string;
    title: string;
    isUpdate: boolean;
}

@Component({
    selector: 'categotyValuesModal',
    templateUrl: 'category-values-modal.component.html'
})
export class CategoryValuesModalComponent extends DialogComponent<DialogModel, CategoryValues> implements DialogModel, AfterViewInit {
    title: string;
    name: string;
    isUpdate: boolean;
    categoryValues: CategoryValues = new CategoryValues();    

    constructor(dialogService: DialogService){ 
        super(dialogService);
    }

    confirm() {
        if (this.name) {
            console.log('confirm', this.categoryValues);
            this.categoryValues.name = this.name;
            console.log('confirm', this.categoryValues);
            this.result = this.categoryValues;
        }
        this.close();
    }   

    ngAfterViewInit() {
        if (name) {
            this.categoryValues.name = name;
        }
    }
}
