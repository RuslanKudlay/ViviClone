import { Component, OnInit } from '@angular/core';
import { QueryBaseModel } from '../../../models/query-models/base-query.model';
import { SliderService } from '../../../services/slider.service';
import { SliderModel } from '../../../models/slider-model';
import { EditSliderButtonComponent } from '../../../extentsions/edit-slider-button.component';
import { DeleteButtonComponent } from '../../../extentsions/delete-button.component';

@Component({
    templateUrl: 'slider-list.component.html',
    styleUrls: ['slider-list.component.css']
})
export class SliderListComponent implements OnInit {
    public queryModel: QueryBaseModel<SliderModel> = new QueryBaseModel<SliderModel>();

    constructor(public sliderService: SliderService) { }

    ngOnInit() {
        this.createColumnDefs();
    }

    public createColumnDefs() {
        return [
            {
                headerName: 'Image',
                field: 'base64Image',
                cellRenderer: this.pasteImage,
                width: 500
            },
            {
                headerName: 'Location',
                field: 'location',
                width: 100
            },
            {
                headerName: 'Name Ware Link',
                field: 'nameWare',
                filter: 'text',
                sortable: true,
                width: 300
            },
            {
                headerName: '',
                field: 'edit',
                cellRendererFramework: EditSliderButtonComponent,
                cellClass: 'grid-cell-centered',
                width: 100
            },
            {
                headerName: '',
                field: 'delete',
                cellRendererFramework: DeleteButtonComponent,
                cellClass: 'grid-cell-centered',
                width: 100
            }
        ];
    }

    private pasteImage(params) {
        if (params.value != undefined) {
            var imageElement = document.createElement("img");
            imageElement.src = params.value;
            imageElement.style.height = "100%";
            imageElement.style.width = "auto";
            return imageElement;
        }
    }
}  
