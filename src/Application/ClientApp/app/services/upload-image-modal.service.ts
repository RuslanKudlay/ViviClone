import { Injectable } from '@angular/core';
import { DialogService } from 'ng2-bootstrap-modal';
import { ImageModalComponent, ImageDialogModel } from '../common/image-modal.component';

@Injectable()
export class UploadImageModalService {
    options: any;
    link: string = '';

    constructor(private dialogService: DialogService) { }

    showModal(): any {
        return this.dialogService.addDialog<void, ImageDialogModel>(ImageModalComponent);       
    };    
}
