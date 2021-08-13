import { Location } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Category } from '../../../models/category.model';
import { CategoryService } from '../../../services/category.service';
import { CreateCyrillicFriendlySuburlService } from '../../../services/create-cyrillic-friendly-suburl.service';
import { ExtensionModalService } from '../../../services/extension-modal-service';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';

@Component({
    selector: 'category',
    templateUrl: './category.component.html'
})
export class CategoryComponent implements OnInit {
    @Input() title?: string;
    public myForm: FormGroup;

    public category: Category = new Category();

    constructor(private categoryService: CategoryService, private route: ActivatedRoute,
        private location: Location, private addModalService: ExtensionModalService, private formBuilder: FormBuilder,
        private createCyrillicFriendlySuburlService: CreateCyrillicFriendlySuburlService) { }

    public ngOnInit(): void {
        this.initForm();
        this.route.params.subscribe(params => {
            const subUrl: string = params['subUrl'];
            if (subUrl == 'newCategory') {
                this.title = 'Add Category';
            } else {
                this.title = 'Update Category';
                this.categoryService.GetBySubUrl(subUrl)
                    .subscribe((response: Category) => {
                        this.category = response;
                        this.initForm(response);
                    });
            }
        });
    }

    public addCategory(): void {
        this.setDataFromForm();
        this.categoryService.Save(this.category).subscribe((response: any) => {
            this.location.back();
        });
    }

    public updateCategory(): void {
        this.setDataFromForm();
        this.categoryService.Update(this.category).subscribe((response: any) => {
            this.location.back();
        });
    }

    private initForm(category?: Category): void {
        if (category) {
            this.myForm = this.formBuilder.group({
                "name": [category.name, [Validators.required]],
                "subUrl": [category.subUrl, [Validators.required]],
                "isEnable": [category.isEnable, []]
            });
        } else {
            this.myForm = this.formBuilder.group({
                "name": ['', [Validators.required]],
                "subUrl": ['', [Validators.required]],
                "isEnable": [false, []]
            });
        }
    }

    private setDataFromForm() {
        this.category.isEnable = this.myForm.value.isEnable;
        this.category.name = this.myForm.value.name;
        this.category.subUrl = this.myForm.value.subUrl;
    }
    public errorDisplayed(controlName: string): boolean {
        return this.myForm.controls[controlName].invalid && this.myForm.controls[controlName].touched;
    }

    public openAddingCategoryValueDialog(name?: string): void {
        let dialogTitle: string;
        let isUpdate: boolean;

        if (name && this.category.categoryValues.find(categoryValue => categoryValue.name == name)) {
            dialogTitle = 'Update Category Value';
            isUpdate = true;
        } else {
            dialogTitle = 'Add Category Value';
            isUpdate = false;
        }

        this.addModalService.showCategoryValuesModal(name, dialogTitle, isUpdate).subscribe((categoryValue) => {
            console.log('showCategoryValuesModal', categoryValue);
            if (categoryValue) {
                console.log(this.category.categoryValues.find(categoryValue => categoryValue.name == name));
                const existingCategoryValue = this.category.categoryValues.find(categoryValue => categoryValue.name == name);
                if (existingCategoryValue) {
                    existingCategoryValue.name = categoryValue.name;
                } else {
                    this.category.categoryValues.push(categoryValue);
                }
            }
        });
    }

    public deleteCategoryValues(index: number): void {
        this.category.categoryValues.splice(index, 1);
    }

    public onKeyCategoryName(newValue: string): void {
        this.myForm.controls['subUrl'].setValue(this.createCyrillicFriendlySuburlService.createSuburl(newValue));
    }

}
