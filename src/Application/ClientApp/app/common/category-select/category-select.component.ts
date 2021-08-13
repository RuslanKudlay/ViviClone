import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Ware } from '../../models/ware.model';
import { CategoryService } from '../../services/category.service';
import { ExtensionModalService } from '../../services/extension-modal-service';
import { CategoryValues } from '../../models/category-values.model';

@Component({
    selector: 'category-select',
    templateUrl: 'category-select.component.html',
    styleUrls: ['./category-select.component.css']
})
export class CategorySelectComponent implements OnInit {
    @Input() ware: Ware;
    public categories: any[] = [];
    public categoriesLeft: any[] = [];
    public categoriesRight: any[] = [];
    searchText: string;
    isSelectedCategoryValues = false;
    isChangeArray = false;

    @Output() onSelectedChanged: EventEmitter<any> = new EventEmitter<any>();
    
    constructor(private categoryService: CategoryService, private modalService: ExtensionModalService) { }

    public ngOnInit(): void {
        this.categoryService.GetAll(null).subscribe(responseCategories => {
            responseCategories.result.forEach(responseCategory => {
                const categoryValues: any[] = [];
                if (responseCategory.categoryValues) {
                    responseCategory.categoryValues.forEach(categoryValue => {
                        const newCategoryValue = {
                            id: categoryValue.id,
                            categoryId: responseCategory.id,
                            name: categoryValue.name,
                            isChecked: this.ware.categoryValues && this.ware.categoryValues.find(c => c.id === categoryValue.id) != null ? true : false
                        };
                        categoryValues.push(newCategoryValue);
                    });
                }
                const newCategory = {
                    category: responseCategory,
                    categoryValues: categoryValues,
                    selectedCategoryValues: categoryValues ? categoryValues.filter(c => c.isChecked == true) : null
                };
                this.categories.push(newCategory);
                if (this.isChangeArray) {
                    this.categoriesLeft.push(newCategory);
                } else {
                    this.categoriesRight.push(newCategory);
                }
                this.isChangeArray = !this.isChangeArray;
            });
        });

        if (this.ware.categoryValues && this.ware.categoryValues.length > 0) {
            this.isSelectedCategoryValues = true;
        }
    }

    public showCategoryValuesSelectModal(selectedCategory): void {
        this.modalService.showCategoryValuesSelectModal(selectedCategory.categoryValues).subscribe(selectedCategoryValues => {
            if (selectedCategoryValues && selectedCategoryValues.length > 0) {
                this.isSelectedCategoryValues = false;
                selectedCategory.categoryValues.forEach(categoryValue => {
                    const isExist = selectedCategoryValues.find(_ => _.id == categoryValue.id);
                    if (isExist) {
                        categoryValue.isChecked = true;
                        this.isSelectedCategoryValues = true;
                    }
                });
                selectedCategory.selectedCategoryValues = selectedCategory.categoryValues ? selectedCategory.categoryValues.filter(c => c.isChecked == true) : null;
            }
            this.onSelectedCategoryValues();
        });
    }

    public removeCategoryValues(selectedCategoryId, categoryValueId): void {
        this.categories.forEach(category => {
            if (category.category.id == selectedCategoryId && category.categoryValues) {
                category.categoryValues.forEach(categoryValue => {
                    if (categoryValue.id == categoryValueId) {
                        categoryValue.isChecked = false;
                    }
                });
                category.selectedCategoryValues = category.categoryValues ? category.categoryValues.filter(c => c.isChecked == true) : null;
            }
        });

        this.onSelectedCategoryValues();        
    }

    private onSelectedCategoryValues(): void {
        const categoryValues: Array<any> = new Array();
        this.categories.forEach(category => {
            category.categoryValues.forEach(element => {
                if (element.isChecked) {
                    const item: CategoryValues = new CategoryValues();
                    item.id = element.id;
                    item.categoryId = category.id;
                    item.name = element.name;
                    categoryValues.push(item);
                }
            });
        });
        this.onSelectedChanged.emit(categoryValues);      
    }

    public getCategoryValuesTitle(categoryValues): string {
        return categoryValues.map(_ => _.name).join(', ');
    }
}
