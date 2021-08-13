import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { GenericMethod } from '../../interfaces/generic.interface';
import { QueryBaseModel } from '../../models/query-models/base-query.model';
import { GridOptions, IDatasource } from 'ag-grid-community';

@Component({
    selector: 'app-ag-grid-helper',
    templateUrl: './ag-grid-helper.component.html',
    styleUrls: ['./ag-grid-helper.component.css']
})
export class AgGridHelperComponent implements OnInit, AfterViewInit {

    public gridOptions: GridOptions;
    public dataSource: IDatasource;
    private isInit: boolean = false;
    private _columnDefs: Array<object>;
    private _service: GenericMethod;
    private _queryModel: any;
    private _rowHeight: number = 40;
    private data: Array<any>;

    @Input('columnDefs') set columnDefs(value: Array<object>) {
        this._columnDefs = value;
        if (!this.isInit && this._columnDefs &&
            this._queryModel && this._service) {
            this.initGrid();
        }
    }

    @Input('service') set service(value: GenericMethod) {
        this._service = value;
        if (!this.isInit && this._columnDefs &&
            this._queryModel && this._service) {
            this.initGrid();
        }
    }

    @Input('rowHeight') set rowHeight(value: number) {
        this._rowHeight = value;

        if (value == null || value == undefined) {
            this._rowHeight = 40;
        }

        if (!this.isInit && this._columnDefs &&
            this._queryModel && this._service) {
            this.initGrid();
        }
    }

    @Input('queryModel') set queryModel(value: QueryBaseModel<any>) {
        this._queryModel = value;
        if (!this.isInit && this._columnDefs &&
            this._queryModel && this._service) {
            this.initGrid();
        }
    }

    constructor() {

    }


    initGrid() {
        this.isInit = true;
        const that = this;
        this.gridOptions = <GridOptions>{
            datasource: that.dataSource,
            columnDefs: that._columnDefs,
            animateRows: true,
            rowSelection: 'single',
            rowDeselection: true,
            rowModelType: 'infinite',
            pagination: true,
            context: this,
            paginationPageSize: 18,
            cacheOverflowSize: 2,
            maxConcurrentDatasourceRequests: 2,
            infiniteInitialRowCount: 1,
            maxBlocksInCache: 1,
            cacheBlockSize: 18,
            rowHeight: this._rowHeight,
            headerHeight: 30,
            paginationAutoPageSize: true,
            getRowNodeId: function (item) {
                return item.id;
            },
            onGridReady: function (params) {
                if (params) {
                    params.api.sizeColumnsToFit();
                }
            }
        };

        const dataSource = {
            getRows: function (params: any) {
                that._queryModel.skip = params.startRow;

                that._queryModel.take = params.endRow - params.startRow;

                that._queryModel.orderBy = "";
                that._queryModel.orderByDesc = "";

                if (params.sortModel.length > 0) {
                    params.sortModel[0].sort == 'asc' ? that._queryModel.orderBy = params.sortModel[0].colId : that._queryModel.orderByDesc = params.sortModel[0].colId;
                }

                // Reset fields in query model.
                for (const field in that._queryModel) {
                    if (field.includes('Contains')) {
                        that._queryModel[field] = '';
                    }
                }

                // Set field in query model.
                for (const filter in params.filterModel) {
                    that._queryModel[filter + 'Contains'] = params.filterModel[filter].filter;
                }

                that._service.GetAll(that._queryModel).subscribe(response => {
                    that.data = response.result;
                    params.successCallback(that.data, response.totalCount);
                });
            }
        };

        this.gridOptions.onGridReady = (event: any) => {
            this.gridOptions.api.setDatasource(dataSource);
        };

        this.gridOptions.onCellClicked = (event: any) => {
            if (event.colDef.field && event.colDef.field == 'delete') {
                this._service.Delete(event.data.id).subscribe((result) => {
                    if (result) {
                        const deletedItem = this.data.find(d => d.id == event.data.id);
                        const index = this.data.indexOf(deletedItem, 0);
                        this.data.splice(index, 1);
                        this.gridOptions.onGridReady(null);
                    } else {
                        alert('Failed');
                    }
                });
            }
        };
    }

    public ngOnInit(): void {

    }

    public ngAfterViewInit(): void {

    }


}
