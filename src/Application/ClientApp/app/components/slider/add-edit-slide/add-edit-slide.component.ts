import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SliderModel } from '../../../models/slider-model';
import { TypeStatus } from '../../../models/slider-model';
import { SliderService } from '../../../services/slider.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UploadImageModalService } from '../../../services/upload-image-modal.service';
import { WareService } from '../../../services/ware.service';
import { WareQueryModel } from '../../../models/query-models/ware-query.model';
import { Ware } from '../../../models/ware.model';
import { environment } from '../../../../environments/environment';

@Component({
    templateUrl: './add-edit-slide.component.html'
})
export class AddEditSliderComponent implements OnInit {

    public title: string = "Loading...";
    public loading: boolean = false;
    public slide: SliderModel = new SliderModel();
    public types: { number: TypeStatus, type: string }[] = [];
    public wares: Ware[];
    public selectedType: TypeStatus;
    public selectedWare: Ware;
    public myForm: FormGroup;
    private slideId: any;
    private readonly ADDITION_TO_URL: string = environment.apiUrl + "Shop/WareDetails?subUrl=";

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private location: Location,
        private sliderService: SliderService,
        private formBuilder: FormBuilder,
        private uploadImageModalService: UploadImageModalService,
        private wareService: WareService
    ) { }

    public ngOnInit(): void {

        this.initForm();
        this.loading = true;
        this.route.params.subscribe(params => {
            this.slideId = params['subUrl'];

            if (this.slideId == 'newSlide') {
                this.setType();
                let query: WareQueryModel = new WareQueryModel();
                this.wareService.GetAll(query).subscribe((response: WareQueryModel) => {
                    this.wares = response.result;
                    this.setLinkToWare();
                    this.title = `Add new slide`;
                    this.loading = false;
                })
            } else {
                this.slideId = parseInt(this.slideId);
                this.sliderService.getSlide(this.slideId).subscribe((response: SliderModel) => {
                    this.slide = response;
                    this.setType();
                    this.initForm(this.slide);
                    let query: WareQueryModel = new WareQueryModel();
                    this.wareService.GetAll(query).subscribe((response: WareQueryModel) => {
                        this.wares = response.result;
                        this.setLinkToWare();
                        this.title = `Update slide #${this.slideId}`;
                        this.loading = false;
                    })
                }, (error: any) => {
                    alert(error);
                });
            }
        });
    }

    private setType() {
        if (this.slide && this.slide.id) {
            if (this.slide.type == TypeStatus.Main) {
                this.selectedType = TypeStatus.Main;
            } else {
                this.selectedType = TypeStatus.Shop;
            }

            this.types.push({ number: TypeStatus.Main, type: TypeStatus[1] });
            this.types.push({ number: TypeStatus.Shop, type: TypeStatus[2] });

        } else {
            this.selectedType = TypeStatus.Main;
            this.types.push({ number: TypeStatus.Main, type: TypeStatus[1] });
            this.types.push({ number: TypeStatus.Shop, type: TypeStatus[2] });
        }
    }

    private setLinkToWare() {
        if (this.slide && this.slide.id) {
            let wareUrl = this.slide.linkToWare.replace(this.ADDITION_TO_URL, "");
            this.selectedWare = this.wares.find(_ => _.subUrl == wareUrl);
        } else {
            this.selectedWare = this.wares[0];
        }
    }

    private initForm(slide?: SliderModel): void {
        if (slide && slide.id) {
            this.myForm = this.formBuilder.group({
                "linkToWare": [slide.linkToWare, [Validators.required]],
                "image": [slide.image, [Validators.required]],
                "type": [slide.type, [Validators.required]]
            });
        } else {
            this.myForm = this.formBuilder.group({
                "linkToWare": ['', [Validators.required]],
                "image": ['', [Validators.required]],
                "type": ['', [Validators.required]]
            });
        }
    }

    public showModalImageUploader(): void {
        this.uploadImageModalService.showModal().subscribe((response) => {
            if (response) {
                this.slide.image = response.link;
                this.slide.base64Image = response.base64Image;
                this.myForm.controls["image"].setValue(response.link);
            }
        });
    }

    public errorDisplayed(controlName: string): boolean {
        return this.myForm.controls[controlName].invalid && this.myForm.controls[controlName].touched;
    }

    public addSlide(): void {
        this.setSlideByForm(this.slide, this.myForm);
        this.sliderService.Save(this.slide).subscribe((response: any) => {
            this.location.back();
        }, (error: any) => {
            alert(error);
        });
    }

    public updateSlide(): void {
        this.setSlideByForm(this.slide, this.myForm);
        this.sliderService.Update(this.slide).subscribe(response => {
            this.location.back();
        }, (error: any) => {
            alert(error);
        });
    }

    private setSlideByForm(slide: SliderModel, form: FormGroup): void {
        slide.image = form.value.image;
        slide.linkToWare = this.ADDITION_TO_URL + form.value.linkToWare.subUrl;
        slide.type = form.value.type;
    }
}
