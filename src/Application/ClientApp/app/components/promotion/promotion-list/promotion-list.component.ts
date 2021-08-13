import { EditPromotionsButtonComponent } from './../../../extentsions/edit-promotion-button.component';
import { Component } from '@angular/core';
import { PromotionService } from '../../../services/promotion.service';
import { PromotionQueryModel } from './../../../models/query-models/promotion-query.model';
import { DeleteButtonComponent } from '../../../extentsions/delete-button.component';

@Component({
    templateUrl: './promotion-list.component.html'
})
export class PromotionListComponent {
    status: string;
    queryModel: PromotionQueryModel = new PromotionQueryModel();

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
                headerName: 'Title',
                field: 'title',
                filter: 'text',
                sortable: true,
                resizable: true,
                width: 150
            },
            {
                headerName: 'Body',
                field: 'body',
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
                headerName: 'Date',
                field: 'date',
                filter: 'text',
                sortable: true,
                resizable: true,
                width: 150,
                cellRenderer: (data) => {
                    return data.value ? (new Date(data.value)).toLocaleDateString() : '';
                }
            },
            {
                headerName: 'Last Update Date',
                field: 'lastUpdateDate',
                filter: 'text',
                sortable: true,
                resizable: true,
                width: 150,
                cellRenderer: (data) => {
                    return data.value ? (new Date(data.value)).toLocaleDateString() : '';
                }
            },
            {
                headerName: '',
                field: 'editDelete',
                cellRendererFramework: EditPromotionsButtonComponent,
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
        public promotionService: PromotionService
    ) { }
}
