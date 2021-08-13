import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Brand } from '../../../models/brand.model';
import { GroupOfWares } from '../../../models/group-of-wares.model';
import { Ware } from '../../../models/ware.model';
import { CreateCyrillicFriendlySuburlService } from '../../../services/create-cyrillic-friendly-suburl.service';
import { ExtensionModalService } from '../../../services/extension-modal-service';
import { GroupOfWaresService } from '../../../services/group-of-wares.service';
import { UploadImageModalService } from '../../../services/upload-image-modal.service';
import { WareService } from '../../../services/ware.service';
import { BrandService } from '../../../services/brand.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
    templateUrl: './add-edit-ware.component.html',
    styleUrls: ['./add-edit-ware.component.css']
})
export class AddEditWareComponent implements OnInit {
    public myForm: FormGroup;
    private id: any;
    private subUrl: string;
    private wareDescription: string;
    public ware: Ware = new Ware();
    private status: string;
    public titleStatus: string;
    public brands: Brand[] = [];
    public groupOfWares: GroupOfWares[] = [];

    constructor(
        private wareService: WareService,
        private groupOfWaresService: GroupOfWaresService,
        private route: ActivatedRoute,
        private location: Location,
        private createCyrillicFriendlySuburlService: CreateCyrillicFriendlySuburlService,
        private uploadImageModalService: UploadImageModalService,
        private errorModal: ExtensionModalService,
        private brandService: BrandService,
        private formBuilder: FormBuilder
    ) { }

    public ngOnInit(): void {
        this.initForm();
        this.route.params.subscribe(params => {
            this.subUrl = params['subUrl'];
            this.titleStatus = 'Add Ware';
            if (this.subUrl && this.subUrl != 'newWare') {
                this.titleStatus = 'Update Ware';
                this.wareService.GetBySubUrl(this.subUrl).subscribe((response: Ware) => {
                    if(response) {
                        this.ware = response;
                        if(!this.ware.gows) {
                            this.ware.gows = [];
                        }
                        this.initForm(response);
                    }
                });
            }
            this.brandService.GetAll(null).subscribe((result) => {
                this.brands = result.result;
            });
            this.groupOfWaresService.getTreeGOW().subscribe((response: GroupOfWares[]) => {
                this.groupOfWares = response;
            });
        });
    }

    private initForm(ware?: Ware): void {
        if (ware) {
            this.myForm = this.formBuilder.group({
                "name": [ware.name, [Validators.required]],
                "subUrl": [ware.subUrl, [Validators.required]],
                "brandId": [ware.brandId, [Validators.required]],
                "isEnable": [ware.isEnable, []],
                "isBestseller": [ware.isBestseller, []],
                "IsOnlyForProfessionals": [ware.isOnlyForProfessionals, []],
                "price": [ware.price, [Validators.required]],
                "vendorCode": [ware.vendorCode, []],
                "metaDescription": [ware.metaDescription, []],
                "metaKeywords": [ware.metaKeywords, []],
            });
        } else {
            this.myForm = this.formBuilder.group({
                "name": ['', [Validators.required]],
                "subUrl": ['', [Validators.required]],
                "brandId": [null, [Validators.required]],
                "isEnable": [false, []],
                "isBestseller": [false, []],
                "IsOnlyForProfessionals": [false, []],
                "price": [0, [Validators.required]],
                "vendorCode": ['', []],
                "metaDescription": ['', []],
                "metaKeywords": ['', []],
            });
        }
    }

    public addWare(): void {
        this.setDataFromForm();
        this.wareService.Save(this.ware).subscribe((response: any) => {
            this.status = 'Added';
            this.location.back();
        }, (error: any) => {
            this.status = 'error ' + error;
        });
    }

    public updateWare(): void {
        this.setDataFromForm();
        this.wareService.Update(this.ware).subscribe((response: any) => {
            this.status = 'Updated';
            this.location.back();
        }, (error: any) => {
            this.status = 'error ' + error;
        });
    }

    public onKeyWareName(newValue: string): void {
        this.myForm.controls['subUrl'].setValue(this.createCyrillicFriendlySuburlService.createSuburl(newValue));
    }


    public wareBodyUpdated(newValue: string): void {
        this.wareDescription = newValue;
    }

    public showModalImageUploader(): void {
        this.uploadImageModalService.showModal().subscribe((response) => {
            if (response) {
                this.ware.wareImage = response.link;
                this.ware.base64Image = response.base64Image;
            }
        });
    }

    public selectedCategoryValues(categoryValues: any): void {
        if (categoryValues) {
            this.ware.categoryValues = categoryValues;
        }
    }

    private setDataFromForm() {
        this.ware.brandId = this.myForm.value.brandId;
        this.ware.name = this.myForm.value.name;
        this.ware.subUrl = this.myForm.value.subUrl;
        this.ware.vendorCode = this.myForm.value.vendorCode;
        this.ware.price = this.myForm.value.price;
        this.ware.metaDescription = this.myForm.value.metaDescription;
        this.ware.metaKeywords = this.myForm.value.metaKeywords;
        this.ware.isEnable = this.myForm.value.isEnable;
        this.ware.isBestseller = this.myForm.value.isBestseller;
        this.ware.isOnlyForProfessionals = this.myForm.controls.IsOnlyForProfessionals.value;
        if(this.wareDescription)
            this.ware.text = this.wareDescription;
    }

    public errorDisplayed(controlName: string): boolean {
        if (controlName == 'price')
            return this.errorPiriceDisplayed();
        else
            return this.myForm.controls[controlName].invalid && this.myForm.controls[controlName].touched;
    }

    public errorPiriceDisplayed(): boolean {
        var price = this.myForm.controls.price.value + '';
        var fraction = price.split('.');
        var priceOverLength = false;
            if (fraction.length > 1 && (fraction[1].length > 2 || fraction.length > 2))
                priceOverLength = true;
        var invalid = (price=='null' || this.myForm.controls.price.value < 0 || priceOverLength) && this.myForm.controls.price.touched
        return invalid;
    }
}
