import { Injectable } from '@angular/core';
import { DialogService } from 'ng2-bootstrap-modal';
import { CategoryValuesModalComponent } from '../components/category/common/category-values-modal.component';
import { IconModalComponent } from '../common/icon-modal.component';
import { CloseChatModalComponent } from '../common/modals/close-chat--modal.component';
import { ErrorModalComponent } from '../common/error-modal/error-modal.component';
import { DeleteConfirmationModalComponent } from '../common/delete-confirmation-modal.component';
import { CategoryValuesSelectModalComponent } from '../common/category-values-select-modal/category-values-select-modal.component';

@Injectable()
export class ExtensionModalService {
    options: any; 
    
    constructor(private dialogService: DialogService) { }

    showCategoryValuesModal(name: string, title: string, isUpdate: boolean): any {        
        return this.dialogService.addDialog(CategoryValuesModalComponent, {name : name , title: title, isUpdate: isUpdate});       
    }  

    public showCategoryValuesSelectModal(categoryValues: any): any {
        return this.dialogService.addDialog(CategoryValuesSelectModalComponent, { categoryValues: categoryValues });    
    }

    showIconModal(): any {
        return this.dialogService.addDialog(IconModalComponent);    
    }

    showErrorModal(errMsg: string): any {
        return this.dialogService.addDialog(ErrorModalComponent, { errMsg: errMsg });    
    }

    showCloseChatModal(): any {
        return this.dialogService.addDialog(CloseChatModalComponent);
    }

    showDeleteConfirmModal(): any {
        return this.dialogService.addDialog(DeleteConfirmationModalComponent);
    }
}
