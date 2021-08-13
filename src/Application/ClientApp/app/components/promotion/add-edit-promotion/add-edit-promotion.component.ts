import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Promotion } from '../../../models/promotion.model';
import { CreateCyrillicFriendlySuburlService } from '../../../services/create-cyrillic-friendly-suburl.service';
import { PromotionService } from '../../../services/promotion.service';

@Component({
    templateUrl: './add-edit-promotion.component.html'
})
export class AddEditPromotionComponent implements OnInit {

    public myForm: FormGroup;
    private subUrl: string;
    public promotion: Promotion = new Promotion();
    private status: string;
    public titleStatus: string;
    private bodyText: string;

    constructor(private promotionService: PromotionService, private route: ActivatedRoute, private formBuilder: FormBuilder,
        private location: Location, private createCyrillicFriendlySuburlService: CreateCyrillicFriendlySuburlService) { }

    public ngOnInit(): void { 
        this.initForm();
        this.route.params.subscribe(params => {
            this.subUrl = params['subUrl'];
            if (this.subUrl && this.subUrl != 'newPromotion') {
                this.titleStatus = 'Update Promotion';
                this.promotionService.GetBySubUrl(this.subUrl)
                    .subscribe((response: Promotion) => {
                        this.promotion = response;
                        this.initForm(this.promotion);
                    }, (error: any) => {
                        this.status = 'error ' + error;
                    });
            } else {
                this.titleStatus = 'Add Promotion';
            }
        });
    }

    private initForm(promotion?: Promotion): void {
        if (promotion) {
            this.myForm = this.formBuilder.group({
                "title": [promotion.title, [Validators.required]],
                "subUrl": [promotion.subUrl, [Validators.required]],
                "isEnable": [promotion.isEnable, []],
                "date": [promotion.date, []],
                "lastUpdateDate": [promotion.lastUpdateDate, []],
                "metaDescription": [promotion.metaDescription, []],
                "metaKeywords": [promotion.metaKeywords, []]
            });
        } else {
            this.myForm = this.formBuilder.group({
                "title": ['', [Validators.required]],
                "subUrl": ['', [Validators.required]],
                "isEnable": [false, []],
                "date": [new Date(), []],
                "lastUpdateDate": [new Date(), []],
                "metaDescription": ['', []],
                "metaKeywords": ['', []]
            });
        }
    }

    public addPromotion(): void {
        this.setPromotionByForm(this.promotion, this.myForm);
        this.promotionService.Save(this.promotion).subscribe((response: any) => {
          this.status = 'Added';
          this.location.back();
        }, (error: any) => {
          this.status = 'error ' + error;
        });
    }

    public updatePromotion(): void {
        this.setPromotionByForm(this.promotion, this.myForm);
        this.promotionService.Update(this.promotion).subscribe((response: any) => {
          this.status = 'Updated';   
          this.location.back();
        }, (error: any) => {
          this.status = 'error ' + error;
        });
    }

    public onKeyPromotionTitle(newValue: string): void {
        this.myForm.controls['subUrl'].setValue(this.createCyrillicFriendlySuburlService.createSuburl(newValue));
    }

    public promotionBodyUpdated(newValue: string): void {
        this.bodyText = newValue;
    }

    private setPromotionByForm(promotion: Promotion, form: FormGroup): void {
        promotion.title = form.value.title;
        promotion.subUrl = form.value.subUrl;
        promotion.isEnable = form.value.isEnable;
        promotion.date = form.value.date;
        promotion.lastUpdateDate = form.value.lastUpdateDate;
        promotion.metaDescription = form.value.metaDescription;
        promotion.metaKeywords = form.value.metaKeywords;
        promotion.body = this.bodyText;
    }

    public errorDisplayed(controlName: string): boolean {
        return this.myForm.controls[controlName].invalid && this.myForm.controls[controlName].touched;
    }
}
