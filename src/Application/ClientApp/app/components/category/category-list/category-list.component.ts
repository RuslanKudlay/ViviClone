import { Component } from '@angular/core';
import { DialogService } from 'ng2-bootstrap-modal';
import { Category } from '../../../models/category.model';
import { CategoryService } from '../../../services/category.service';
import { CategoryQueryModel } from '../../../models/query-models/category-query.model';
import { EditCategoryButtonComponent } from '../../../extentsions/edit-category-button.component';
import { DeleteButtonComponent } from '../../../extentsions/delete-button.component';

@Component({
    selector: 'category-edit',
    templateUrl: './category-list.component.html',
    providers: [CategoryService]
})
export class CategoryListComponent {
    queryModel: CategoryQueryModel = new CategoryQueryModel();

    public createColumnDefs() {
        return [
            {
                headerName: 'Id',
                field: 'id',
                filter: 'number',
                sortable: true,
                resizable: true,
                width: 75
            },
            {
                headerName: 'Name',
                field: 'name',
                filter: 'text',
                sortable: true,
                resizable: true,
                width: 150
            },
            {
                headerName: 'Is Enable',
                field: 'isEnable',
                filter: 'text',
                sortable: true,
                resizable: true,
                width: 75
            },
            {
                headerName: '',
                field: 'editDelete',
                cellRendererFramework: EditCategoryButtonComponent,
                width: 75
            },
            {
                headerName: '',
                field: 'delete',
                cellRendererFramework: DeleteButtonComponent,
                width: 75
            }
        ];
    }

    constructor(
        public categoryService: CategoryService
    ) { }
}
