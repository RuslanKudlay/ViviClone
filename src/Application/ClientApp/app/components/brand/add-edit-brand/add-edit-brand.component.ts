import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Brand } from '../../../models/brand.model';
import { BrandService } from '../../../services/brand.service';
import { CreateCyrillicFriendlySuburlService } from '../../../services/create-cyrillic-friendly-suburl.service';
import { UploadImageModalService } from '../../../services/upload-image-modal.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  templateUrl: './add-edit-brand.component.html'
})
export class AddEditBrandComponent implements OnInit {
    public myForm: FormGroup;
    private subUrl: string;
    public brand: Brand = new Brand();
    private status: string;
    public title: string;
    private bodyText: string;
    private isBodyTextDitry: boolean = false;

    public showColorPicker: boolean = false;
    public showColorTextPicker: boolean = false;

    constructor(private brandService: BrandService, private route: ActivatedRoute, private location: Location, private formBuilder: FormBuilder,
        private createCyrillicFriendlySuburlService: CreateCyrillicFriendlySuburlService, private uploadImageModalService: UploadImageModalService) { }

    public ngOnInit(): void {
        this.initForm();
        this.brand.colorTitle = '#fff';
        this.brand.color = '#fff';
        this.route.params.subscribe(params => {
            this.subUrl = params['subUrl'];
            if (this.subUrl && this.subUrl != 'newBrand') {
                this.title = 'Update Brand';
                this.brandService.GetBySubUrl(this.subUrl)
                    .subscribe((response: Brand) => {
                        this.brand = response;
                        this.initForm(this.brand);
                    }, (error: any) => {
                        this.status = 'error ' + error;
                    });
            } else {
                this.title = 'Add Brand';
            }
        });
    }

    private initForm(brand?: Brand): void {
        if (brand) {
            this.myForm = this.formBuilder.group({
                "name": [brand.name, [Validators.required]],
                "subUrl": [brand.subUrl, [Validators.required]],
                "position": [brand.position, [Validators.required]],
                "isEnable": [brand.isEnable, []],
                "color": [brand.color, [Validators.required]],
                "colorTitle": [brand.colorTitle, [Validators.required]],
                "shortDescription": [brand.shortDescription, [Validators.required]],
                "metaDescription": [brand.metaDescription, []],
                "metaKeywords": [brand.metaKeywords, []]
            });
        } else {
            this.myForm = this.formBuilder.group({
                "name": ['', [Validators.required]],
                "subUrl": ['', [Validators.required]],
                "position": [0, [Validators.required]],
                "isEnable": [false, []],
                "color": ['#ffffff', [Validators.required]],
                "colorTitle": ['#ffffff', [Validators.required]],
                "shortDescription": ['', [Validators.required]],
                "metaDescription": ['', []],
                "metaKeywords": ['', []]
            });
        }
    }

    public addBrand(): void {
        this.setBrandByForm(this.brand, this.myForm);
        this.brandService.Save(this.brand).subscribe((response: any) => {
            this.status = 'Added';
            this.location.back();
        }, (error: any) => {
            this.status = 'error ' + error;
        });
    }

    public updateBrand(): void {
        this.setBrandByForm(this.brand, this.myForm);
        this.brandService.Update(this.brand).subscribe(response => {
            this.status = 'Updated';
            this.location.back();
        });
    }

    public onKeyBrandName(newValue: string): void {
        this.myForm.controls['subUrl'].setValue(this.createCyrillicFriendlySuburlService.createSuburl(newValue));
    }

    public showCP(): boolean {
        return this.showColorPicker = !this.showColorPicker;
    }

    public showCTP(): boolean {
        return this.showColorTextPicker = !this.showColorTextPicker;
    }

    public showModalImageUploader(): void {
        this.uploadImageModalService.showModal().subscribe((response) => {
            if (response) {
                this.brand.logoImage = response.link;
                this.brand.base64Image = response.base64Image;
            }
        });
    }

    public brandBodyUpdated(newValue: string): void {
        this.bodyText = newValue;
        this.isBodyTextDitry = true;
    }

    private setBrandByForm(brand: Brand, form: FormGroup): void {
        brand.name = form.value.name;
        brand.subUrl = form.value.subUrl;
        brand.position = form.value.position;
        brand.isEnable = form.value.isEnable;
        brand.shortDescription = form.value.shortDescription;
        brand.metaDescription = form.value.metaDescription;
        brand.metaKeywords = form.value.metaKeywords;
        brand.body = this.isBodyTextDitry ? this.bodyText : brand.body;
    }

    public errorDisplayed(controlName: string): boolean {
        return this.myForm.controls[controlName].invalid && this.myForm.controls[controlName].touched;
    }
}
