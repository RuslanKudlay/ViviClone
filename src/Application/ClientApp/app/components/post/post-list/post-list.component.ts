import { Component } from '@angular/core';
import { PostQueryModel } from '../../../models/query-models/post-query.model';
import { BlogService } from '../../../services/blog.service';
import { DeleteButtonComponent } from '../../../extentsions/delete-button.component';
import { EditPostButtonComponent } from '../../../extentsions/edit-post-button-component';

@Component({
    templateUrl: './post-list.component.html'
})
export class PostListComponent {
    queryModel: PostQueryModel = new PostQueryModel();

    constructor(public blogService: BlogService) { }

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
                width: 200
            },
            {
                headerName: 'Is Enable',
                field: 'isEnable',
                filter: 'text',
                sortable: true,
                resizable: true,
                width: 100
            },
            {
                headerName: 'Date',
                field: 'dateCreated',
                filter: 'text',
                sortable: true,
                resizable: true,
                width: 200,
                cellRenderer: (data) => {
                    return data ? (new Date(data.value)).toLocaleDateString() : '';
                }
            },
            {
                headerName: 'Last Update Date',
                field: 'dateModificated',
                filter: 'text',
                sortable: true,
                resizable: true,
                width: 200,
                cellRenderer: (data) => {
                    return data.value ? (new Date(data.value)).toLocaleDateString() : '';
                }
            },
            {
                headerName: '',
                field: 'editDelete',
                cellRendererFramework: EditPostButtonComponent,
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
