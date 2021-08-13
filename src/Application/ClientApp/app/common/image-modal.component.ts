import { HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';
import { FileItem, FileUploader, FilterFunction, ParsedResponseHeaders } from 'ng2-file-upload';
import { ImageService } from '../services/image.service';
import { AuthService } from '../services/auth.service';
import { environment } from '../../environments/environment';

export interface ImageDialogModel {
    base64Image: string;
    link: string;
}

@Component({
    templateUrl: './image-modal.component.html',
})
export class ImageModalComponent extends DialogComponent<void, ImageDialogModel> {
    title: string = 'Select Image';
    base64Image: string = '';
    link: string = '';
    picture: any;
    headers: any = new HttpHeaders({ 'Content-Type': 'application/json' });
    public uploader: FileUploader;
    public hasBaseDropZoneOver: boolean = false;
    public hasAnotherDropZoneOver: boolean = false;
    public isAddingEnabled: boolean;

  constructor(
    dialogService: DialogService,
    private imageService: ImageService,
    private authService: AuthService
  ) {
        super(dialogService);
        this.initUploader();
    }

    onSuccessItem(item: FileItem, response: string, status: number, headers: ParsedResponseHeaders): any {
        this.link = JSON.parse(response).link;
        this.setBase64Image(this.link);
        this.isAddingEnabled = true;
        this.initUploader();
    }

    onAfterAddingFile(item: FileItem) {
        //It MUST be redone, since even if admin
        //press the "cancel" button this image already
        //in the data base. It might lead to a lot of redundant
        //images in the [BeautyDneprCoreDevTest].[dbo].[Images] table
        item.withCredentials = false;
        this.uploader.queue[0].upload();
    }

    onErrorItem = function (fileItem: any, response: any, status: any, headers: any) {
        this.isAddingEnabled = false;
        alert('The file could not be downloaded. Please try again.');
    };

    confirm() {
        this.result = { base64Image: this.base64Image, link: this.link };
        this.close();
    }

    private initUploader() {
        let url = environment.apiUrl + "api/uploadImage";
        this.uploader = new FileUploader({ url: url, method: "POST" });
        this.uploader.onSuccessItem = (item, response, status, headers) => this.onSuccessItem(item, response, status, headers);
        this.uploader.onAfterAddingFile = (item) => this.onAfterAddingFile(item);
        this.uploader.onErrorItem = (item, response, status, headers) => this.onErrorItem(item, response, status, headers);
        this.uploader.options.filters = new Array<FilterFunction>();
        this.uploader.authToken = 'Bearer ' + this.authService.getToken();
        // Add filter for upload file
        this.uploader.options.filters.push({
            name: 'sizeFilter',
            fn: function (item: any, options) {
                let fileSize = item.size;
                fileSize = parseInt(fileSize) / (1024);
                if (fileSize <= 5000)
                    return true;
                else {
                    alert('The file size is more than 5000 KB. Please choose another file.');
                    return false;
                }
            }
        });

        this.uploader.options.filters.push({
            name: 'extensionFilter',
            fn: function (item: any, options) {
                const filename = item.name;
                const extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
                if (extension == 'jpg' || extension == 'png' || extension == 'bmp' || extension == 'jpeg')
                    return true;
                else {
                    alert('Invalid file format. Please, choose jpg/jpeg/bmp/png.');
                    return false;
                }
            }
        });
    }

    public fileOverAnother(e: any): void {
        this.hasAnotherDropZoneOver = e;
    }

    public fileOverBase(e: any): void {
        this.hasBaseDropZoneOver = e;
    }

    private setBase64Image(link: string) {
        link = link.substring(1, link.length);
        this.imageService.getImage(link).subscribe((response) => {
            if (response) {
                this.base64Image = response as string;
            } else {
                alert('Server error');
            }
        });
    }
}
