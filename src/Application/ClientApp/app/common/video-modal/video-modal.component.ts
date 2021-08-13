import { Component, OnInit } from '@angular/core';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';

export interface VideDialogModel {
  id: string
}

@Component({
  templateUrl: './video-modal.component.html'
})
export class VideoModalComponent extends DialogComponent<void, VideDialogModel> {

  url: string
  constructor(
    dialogService: DialogService,
  ) {
    super(dialogService);
  }

  findId() {
    var video_id = this.url.split('v=')[1];
    var ampersandPosition = video_id.indexOf('&');
    if (ampersandPosition !== -1) {
      video_id = video_id.substring(0, ampersandPosition);
    }
    return video_id
  }

  confirm() {
    this.result = { id: this.findId() };
    this.close();
  }

}
