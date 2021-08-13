import { style } from '@angular/animations';
import { Component, EventEmitter, Input, OnDestroy, Output } from '@angular/core';
import { DialogService } from 'ng2-bootstrap-modal';
import { UploadImageModalService } from '../services/upload-image-modal.service';
import { VideoModalService } from '../services/video-modal.service';

declare var tinymce: any;
declare function reloadImages(): any;

@Component({
    selector: 'tinymce-editor-component',
    templateUrl: `./tinymce-editor.component.html`
})
export class TinymceEditorComponent implements OnDestroy {
    @Input() elementId: String;
    @Input() content: String;
    @Output() onEditorContentChange = new EventEmitter();
    options: any;
    editor;

    constructor(private dialogService: DialogService,
         private uploadImageModalService: UploadImageModalService,
         private videoModalService:VideoModalService
         ) {
    }

    ngOnChanges(changes: any) {
        this.updateEditor();
    }

    updateEditor() {
        if (this.content) {
            this.editor.setContent(this.content);
        }
    }

    ngAfterViewInit() {
        const that = this;

        tinymce.init({
            selector: '#' + this.elementId,
            skin_url: '/assets/tinymce/skins/lightgray',
            height: '250',
            plugins: 'link code',
            relative_urls: false,
            toolbar: 'undo redo | bold italic |formats| alignleft aligncenter alignright | code | addImagebutton | addVideoEmbedButton',
            setup: editor => {
                this.editor = editor;
                editor.on('init', () => {
                    this.updateEditor();
                });
                editor.on('keyup change', () => {
                    const content = editor.getContent();
                    this.onEditorContentChange.emit(content);
                });
                editor.addButton('addImagebutton', {
                    icon: 'image',
                    onclick: function (e) {
                        that.uploadImageModalService.showModal().subscribe((response) => {
                            if (response) {
                                // response.link contains string like "/api/image/15"
                                editor.insertContent(`<img class="loadImage img-responsive" style="width: 80%" data-imageUrl="${response.link}" src="${response.base64Image}"/>`);
                                // We call reloadImages for asynchronous image loading (imageLoader.js)
                                reloadImages();
                                const content = editor.getContent();
                                if (this.onEditorContentChange)
                                    this.onEditorContentChange.emit(content);
                            }
                        });
                    }
                });
                editor.addButton('addVideoEmbedButton' , {
                    icon: 'link',
                    onclick: function(e) {
                        that.videoModalService.showModal().subscribe((response) => {
                            if(response) {
                                editor.insertContent(`<iframe type='text/html' width='640' height='385'  src='http://www.youtube.com/embed/${response.id}?autoplay=0' frameborder='0'></iframe>`);
                            }
                        })               
                    }
                })
            }
        });
    }

    ngOnDestroy() {
        tinymce.remove(this.editor);
    }
}
