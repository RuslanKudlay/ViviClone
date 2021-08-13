import { Component, OnInit } from '@angular/core';
import { DeleteButtonComponent } from '../../../extentsions/delete-button.component';
import { EditOrderButtonComponent } from '../../../extentsions/edit-order-button.component';
import { OrderQueryModel } from '../../../models/query-models/order-query.model';
import { OrderService } from '../../../services/order.service';
import { formatDate } from '@angular/common';

@Component({
    templateUrl: 'order-list.component.html',
    styleUrls: ['order-list.component.css']
})
export class OrderListComponent implements OnInit {
    public queryModel: OrderQueryModel = new OrderQueryModel();

    constructor(public orderService: OrderService) { }

    ngOnInit() {
        this.createColumnDefs();
    }

    public createColumnDefs() {
        return [
            {
                headerName: 'OrderNumber',
                field: 'orderNumber',
                cellRenderer: 'agGroupCellRenderer',
                sortable: true,
                resizable: true
            },
            {
                headerName: 'Count Of Wares',
                field: 'countOfWares',
                sortable: true,
                resizable: true
            },
            {
                headerName: 'Status',
                field: 'status',
            },
            {
                headerName: 'User',
                field: 'user.name',
                sortable: true,
                resizable: true
            },
            {
                headerName: 'Created date',
                field: 'createdDate',
                sortable: true,
                resizable: true,
                cellRenderer: (data) => {
                    return data.value ? (new Date(data.value)).toLocaleDateString() : '';
                }
            },
            {
                headerName: 'DeliveryService',
                field: 'deliveryService',
                sortable: true,
                resizable: true
            },
            {
                headerName: 'DeclarationNumber',
                field: 'declarationNumber',
                sortable: true,
                resizable: true
            },
            {
                headerName: '',
                field: 'edit',
                cellRendererFramework: EditOrderButtonComponent,
                width: 100
            },
            {
                headerName: '',
                field: 'delete',
                cellRendererFramework: DeleteButtonComponent,
                width: 100
            }
        ];
    }
}  
