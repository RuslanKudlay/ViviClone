import { Injectable } from '@angular/core';
import { DialogService } from 'ng2-bootstrap-modal';
import { VideDialogModel, VideoModalComponent } from '../common/video-modal/video-modal.component';

@Injectable({
  providedIn: 'root'
})
export class VideoModalService {

  constructor(private dialogService: DialogService) { }
  showModal(): any {
    return this.dialogService.addDialog<void, VideDialogModel>(VideoModalComponent);       
};    
}
