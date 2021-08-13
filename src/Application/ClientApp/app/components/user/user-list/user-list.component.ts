import { Component, OnInit } from '@angular/core';
import { UserQueryModel } from '../../../models/query-models/user-query.model';
import { UserService } from '../../../services/user.service';
import { EditUserButtonComponent } from '../../../extentsions/edit-user-button.component';
import { UserStatus } from '../../../models/user-model';

@Component({
    templateUrl: 'user-list.component.html',
    styleUrls: ['user-list.component.css']
})
export class UserListComponent implements OnInit {
    public queryModel: UserQueryModel = new UserQueryModel();

    constructor(public userService: UserService) { }

    ngOnInit() {
        this.createColumnDefs();
    }

    public createColumnDefs() {
        return [
            {
                headerName: 'Name',
                field: 'name',
            },
            {
                headerName: 'Email',
                field: 'email',
                filter: 'agTextColumnFilter',
                filterParams: {
                    filterOptions: ['contains'],
                    suppressAndOrCondition: true
                },
            },
            {
                headerName: 'Role',
                field: 'role',
            },
            {
                headerName: 'Status',
                field: 'status',
                cellRenderer: (data) => {
                    return UserStatus[data.value];
                }
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
                headerName: '',
                field: 'edit',
                cellRendererFramework: EditUserButtonComponent,
                width: 125
            }
        ];
    }
}  
