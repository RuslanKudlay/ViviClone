import { EditAnnouncementButtonComponent } from './../../../extentsions/edit-announcement-button.component';
import { AddEditAnnouncementComponent } from './../add-edit-announcement/add-edit-announcement.component';
import { AnnouncementQueryModel } from './../../../models/query-models/announcement-query.model';
import { Component } from '@angular/core';
import { Announcement } from '../../../models/announcement.model';
import { AnnouncementService } from '../../../services/announcement.service';
import { DeleteButtonComponent } from '../../../extentsions/delete-button.component';

@Component({
    templateUrl: './announcement-list.component.html'
})
export class AnnouncementListComponent {
    status: string;
    queryModel: AnnouncementQueryModel = new AnnouncementQueryModel();

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
                cellRendererFramework: EditAnnouncementButtonComponent,
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
        public announcementService: AnnouncementService
    ) {
    }
}
