import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Category } from '../../../models/category.model';
import { Ware } from '../../../models/ware.model';
import { WaresCategoryValues } from '../../../models/wares-category-value.model';
import { CategoryService } from '../../../services/category.service';
import { WareService } from '../../../services/ware.service';
import { WaresCategoryValuesService } from '../../../services/wares-category-values.service';

@Component({
    templateUrl: './wares-category-values.component.html'
})
export class WaresCategoryValuesComponent {

    sub: any;
    private id: any;
    wares: Array<Ware> = new Array();
    categories: Array<Category> = new Array();
    waresCategoryValue: WaresCategoryValues = new WaresCategoryValues();

    status: string;
    titleStatus: string;

    /* Select Setting*/
  
    itemList: Array<any> = new Array();
    selectedItems: Array<any> = new Array();
    settings = {};
    isDataCome: boolean;
    /* END Select Setting */

    constructor(private wareService: WareService, private route: ActivatedRoute, private location: Location,
        private categoryService: CategoryService, private waresCategoryValuesService: WaresCategoryValuesService) { }

    private ngOnInit() {
        this.isDataCome = false;
        this.wareService.GetAll(null).subscribe(response => {
            this.wares = response;
        });
        this.sub = this.route.params.subscribe(params => {
            this.id = +params['id'];
            this.titleStatus = 'Add Wares Category Values';
        });

        if (!isNaN(this.id) && this.id != 0) {
            this.titleStatus = 'Update Wares Category Values';
            this.getById(this.id);
        }

        this.settings = {
            singleSelection: false,
            text: 'Select Category Value',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            searchPlaceholderText: 'Search Category Value',
            enableSearchFilter: true,
            groupBy: 'category'
        };
        this.categoryService.GetAll(null).subscribe(response => {
            this.categories = response;
            this.categories.forEach(category => {
                category.categoryValues.forEach(categoryValues => {
                    const item = {
                        'id': categoryValues.id,
                        'itemName': categoryValues.name,
                        'category': category.name
                    };
                    this.itemList.push(item);
                    this.isDataCome = true;
                });
            });
        });
    }

    add() {
        this.getCategoryValues();
        this.waresCategoryValuesService.add(this.waresCategoryValue).subscribe((response: any) => {
            this.status = 'Added';
            this.location.back();
        }, (error: any) => {
            this.status = 'error ' + error;
        });
    }

    update() {
        this.getCategoryValues();
        this.waresCategoryValuesService.update(this.waresCategoryValue).subscribe((response: any) => {
            this.status = 'Updated';
            this.location.back();
        }, (error: any) => {
            this.status = 'error ' + error;
        });
    }

    getById(id: number) {
        this.waresCategoryValuesService.getById(id)
            .subscribe((response: WaresCategoryValues) => {
                this.waresCategoryValue = response;
            }, (error: any) => {
                this.status = 'error ' + error;
            });
    }

    onChangeSelectWare(newValue: any) {
        this.waresCategoryValue.ware.id = newValue.id;
    }

    getCategoryValues() {
        this.selectedItems.forEach(element => {
            let categoryValue;
            this.categories.forEach(category => {
                categoryValue = category.categoryValues.find(x => x.categoryId == element.id);
            });
            this.waresCategoryValue.categoryValue.push(categoryValue);
        });
    }
}
