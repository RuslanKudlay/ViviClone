import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PostModel } from '../../../models/post.model';
import { BlogService } from '../../../services/blog.service';
import { CreateCyrillicFriendlySuburlService } from '../../../services/create-cyrillic-friendly-suburl.service';
import { ImageService } from '../../../services/image.service';
import { UploadImageModalService } from '../../../services/upload-image-modal.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
    templateUrl: './add-edit-post.component.html'
})
export class AddEditPostComponent implements OnInit {

    public myForm: FormGroup;
    private subUrl: string = '';
    public post: PostModel = new PostModel();
    public titleStatus: string = '';
    private bodyText: string;

    constructor(private blogService: BlogService, private route: ActivatedRoute, private location: Location, private formBuilder: FormBuilder,
        private createCyrillicFriendlySuburlService: CreateCyrillicFriendlySuburlService, private uploadImageModalService: UploadImageModalService) { }

    public ngOnInit(): void {
        this.initForm();

        this.route.params.subscribe(params => {
            this.subUrl = params['subUrl'];
            if (this.subUrl && this.subUrl != 'newPost') {
                this.titleStatus = 'Update Post';
                this.blogService.GetBySubUrl(this.subUrl).subscribe((response: PostModel) => {
                    this.post = response;
                    this.initForm(this.post);
                }, (error: any) => {
                    alert('Server error: ' + error);
                });
            } else {
                this.titleStatus = 'Add Post';
            }
        });
    }

    private initForm(post?: PostModel): void {
        if (post) {
            this.myForm = this.formBuilder.group({
                "title": [post.title, [Validators.required]],
                "subUrl": [post.subUrl, [Validators.required]],
                "isEnable": [post.isEnable, [Validators.required]],
                "dateCreated": [post.dateCreated, []],
                "dateModificated": [post.dateModificated, []],
                "metaDescription": [post.metaDescription, []],
                "metaKeywords": [post.metaKeywords, []]
            });
        } else {
            this.myForm = this.formBuilder.group({
                "title": ['', [Validators.required]],
                "subUrl": ['', [Validators.required]],
                "isEnable": [false, [Validators.required]],
                "dateCreated": [new Date(), []],
                "dateModificated": [new Date(), []],
                "metaDescription": ['', []],
                "metaKeywords": ['', []]
            });
        }
    }

    public saveOrUpdate(): void {
        this.setPostByForm(this.post, this.myForm);
        if (this.post.id > 0) {
            this.blogService.Update(this.post).subscribe((response: any) => {
                if (response)
                    this.location.back();
            }, (error: any) => {
                alert('Server error: ' + error.message);
            });
        } else {
            this.blogService.Save(this.post).subscribe((response: any) => {
                if (response.id > 0)
                    this.location.back();
            }, (error: any) => {
                alert('Server error: ' + error.message);
            });
        }
    }

    public onKeyPostTitle(newValue: string): void {
        this.myForm.controls['subUrl'].setValue(this.createCyrillicFriendlySuburlService.createSuburl(newValue));
    }

    public postBodyUpdated(newValue: string): void {
        this.bodyText = newValue;
    }

    public showModalImageUploader(): void {
        this.uploadImageModalService.showModal().subscribe((response) => {
            if (response && response.link) {
                this.post.imageURL = response.link;
                this.post.base64Image = response.base64Image;
            }
        });
    }

    private setPostByForm(post: PostModel, form: FormGroup): void {
        post.title = form.value.title;
        post.subUrl = form.value.subUrl;
        post.isEnable = form.value.isEnable;
        post.dateCreated = form.value.dateCreated;
        post.dateModificated = form.value.dateModificated;
        post.metaDescription = form.value.metaDescription;
        post.metaKeywords = form.value.metaKeywords;
        if(this.bodyText)
            post.body = this.bodyText;
    }

    public errorDisplayed(controlName: string): boolean {
        return this.myForm.controls[controlName].invalid && this.myForm.controls[controlName].touched;
    }
}
