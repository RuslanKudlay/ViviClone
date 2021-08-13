import { Component } from '@angular/core';
import { Brand } from '../../../models/brand.model';
import { BrandQueryModel } from '../../../models/query-models/brand-query.model';
import { BrandService } from '../../../services/brand.service';
import { DeleteButtonComponent } from '../../../extentsions/delete-button.component';
import { EditBrandButtonComponent } from '../../../extentsions/edit-brand-button.component';

@Component({
    templateUrl: './brand-list.component.html'
})
export class BrandListComponent {
    status: string;
    queryModel: BrandQueryModel = new BrandQueryModel();

    constructor(
        public brandService: BrandService
    ) { }

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
                headerName: 'Position',
                field: 'position',
                filter: 'text',
                sortable: true,
                resizable: true,
                width: 75
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
                headerName: 'Short Description',
                field: 'shortDescription',
                filter: 'text',
                sortable: true,
                resizable: true,
                width: 200
            },
            {
                headerName: 'Meta Keywords',
                field: 'metaDescription',
                filter: 'text',
                sortable: true,
                resizable: true,
                width: 200
            },
            {
                headerName: 'Meta Description',
                field: 'metaKeywords',
                filter: 'text',
                sortable: true,
                resizable: true,
                width: 200
            },
            {
                headerName: '',
                field: 'editDelete',
                cellRendererFramework: EditBrandButtonComponent,
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
}
