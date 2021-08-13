import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Papa, ParseResult } from 'ngx-papaparse';
import { WareQueryModel } from '../../../models/query-models/ware-query.model';
import { Ware } from '../../../models/ware.model';
import { WareService } from '../../../services/ware.service';
import { DeleteButtonComponent } from '../../../extentsions/delete-button.component';
import { EditWareButtonComponent } from '../../../extentsions/edit-ware-button.component';
import * as XLSX from 'xlsx';
import { forkJoin } from 'rxjs';
import { CategoryValuesService } from '../../../services/category-values.service';
import { GroupOfWaresService } from '../../../services/group-of-wares.service';
import { CategoryValues } from '../../../models/category-values.model';
import { find } from 'rxjs-compat/operator/find';

@Component({
    templateUrl: './ware-list.component.html',
    styleUrls: ['./ware-list.component.css']
})
export class WareListComponent {
    status: string;
    queryModel: WareQueryModel = new WareQueryModel();

    constructor(public wareService: WareService, private papa: Papa, private router: Router, private gowsService: GroupOfWaresService,
        private categoryValuesService: CategoryValuesService) { }

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
                width: 500
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
                headerName: 'Price',
                field: 'price',
                filter: 'number',
                sortable: true,
                resizable: true,
                width: 75
            },
            {
                headerName: 'Vendor Code',
                field: 'vendorCode',
                filter: 'text',
                sortable: true,
                resizable: true,
                width: 100
            },
            {
                headerName: 'Brand',
                field: 'brandName',
                filter: 'text',
                sortable: true,
                resizable: true,
                width: 75
            },
            {
                headerName: 'Is Bestseller',
                filter: 'text',
                field: 'isBestseller',
                sortable: true,
                resizable: true,
                width: 100,
                cellRenderer: params => {
                    return `<input disabled="disabled" type='checkbox' ${params.value ? 'checked' : ''} />`;
                }
            },
            {
                headerName: '',
                field: 'editDelete',
                cellRendererFramework: EditWareButtonComponent,
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

    private initGowsAndCategoryValues(wares) : void {
        forkJoin(
          this.gowsService.getGowsByBrand(),
          this.categoryValuesService.GetAll(new CategoryValues())
        ).subscribe(([gows, categoryValues]) => {
          wares.forEach((item) => {
            item.gows = item.gows
              ? item.gows
                  .split(",")
                  .filter((x) => gows.find((gow) => gow.name === x))
                  .map((value) => gows.find((gow) => gow.name === value))
              : null;
            item.categoryValues = item.categoryValues
              ? item.categoryValues
                  .split(",")
                  .filter((x) =>
                    categoryValues.find(
                      (categoryValue) => categoryValue.name === x
                    )
                  )
                  .map((value) =>
                    categoryValues.find((catValue) => catValue.name === value)
                  )
              : null;
          });
          this.wareService.setWaresForImport(wares);
          this.router.navigate(["/wares/import"]);
        });
    }

    public fileChangeListener(event: any): void {
        const files: FileList = this.getTarget(event).files;

        if (files && files.length > 0) {

            const file: File = files.item(0);

            let data, header;
            const target: DataTransfer = <DataTransfer>event.target;
            const isExcelFile = !!target.files[0].name.match(/(.xls|.xlsx)/);
            const reader: FileReader = new FileReader();
            if (isExcelFile) {
                reader.onload = (e: any) => {
                    /* read workbook */
                    const bstr: string = e.target.result;
                    const wb: XLSX.WorkBook = XLSX.read(bstr, { type: 'binary' });
            
                    /* grab first sheet */
                    const wsname: string = wb.SheetNames[0];
                    const ws: XLSX.WorkSheet = wb.Sheets[wsname];
            
                    /* save data */
                    data = XLSX.utils.sheet_to_json(ws);
                    let waresResult = data.map(item => ({
                        id: +item.id,
                        isEnable: JSON.parse(item.isEnable),
                        vendorCode: item.vendorCode,
                        label: item.label,
                        wareImage: item.wareImage,
                        base64Image: item.base64Image,
                        name: item.name,
                        text: item.text,
                        price: item.price,
                        subUrl: item.subUrl,
                        metaKeywords: item.metaKeywords,
                        metaDescription: item.metaDescription,
                        gows: item.gows ? item.gows : null,
                        categoryValues: item.categoryValues ? item.categoryValues : null,
                        averageRate: +item.averageRate,
                        brandId: +item.brandId,
                        brandName: item.brandName
                    }));

                    this.initGowsAndCategoryValues(waresResult)
                  };
                  reader.readAsBinaryString(target.files[0]);

            } else {
                reader.readAsText(file);


                reader.onload = (e) => {
    
                    const csv: string = reader.result as string;
    
                    this.csvJSON(csv, (jsonObj: string) => {
    
                        const parsedWares: Ware[] = JSON.parse(jsonObj).map(item => ({
                            id: +item.id,
                            isEnable: JSON.parse(item.isEnable),
                            vendorCode: item.vendorCode,
                            label: item.label,
                            wareImage: item.wareImage,
                            base64Image: item.base64Image,
                            name: item.name,
                            text: item.text,
                            price: item.price,
                            subUrl: item.subUrl,
                            metaKeywords: item.metaKeywords,
                            metaDescription: item.metaDescription,
                            gows: item.gows ? item.gows : null,
                            categoryValues: item.categoryValues ? item.categoryValues : null,
                            averageRate: +item.averageRate,
                            brandId: +item.brandId,
                            brandName: item.brandName
                        }));
                        this.wareService.setWaresForImport(parsedWares);
                        this.router.navigate(['/wares/import']);
                    });
                };
            }
        }
    }

    private getTarget(e: any): any {
        let targ;
        if (e.target) {
            targ = e.target;
        } else if (e.srcElement) {
            targ = e.srcElement;
        }
        if (targ.nodeType == 3) targ = targ.parentNode; // defeat Safari bug
        return targ;
    }

    private csvJSON(csvText: string, complete: (obj: string) => void): void {
        this.papa.parse(csvText, {
            complete: (result: ParseResult) => {
                const jsonObj = [];
                for (let i = 1; i < result.data.length; i++) {
                    const obj = {};
                    for (let j = 0; j < result.data[i].length; j++) {
                        obj[result.data[0][j]] = result.data[i][j];
                    }
                    jsonObj.push(obj);
                }
                complete(JSON.stringify(jsonObj));
            }
        });
    }

    public export(): void {
        this.wareService.GetAll(new WareQueryModel()).subscribe((data: any) => {
            const wares: Ware[] = data.result;
            this.download('wares', this.papa.unparse(wares));
        });
    }

    // Creates a file with the specified name and data and downloads it to the user�s computer.
    private download(filename: string, data: string) {
        const blob = new Blob([data], { 'type': 'text/csv;charset=utf8;' });

        if (navigator.msSaveBlob) {
            navigator.msSaveBlob(blob, filename.replace(/ /g, '_') + '.csv');
        } else {
            const link = document.createElement('a');

            link.href = URL.createObjectURL(blob);

            link.setAttribute('visibility', 'hidden');
            link.download = filename.replace(/ /g, '_') + '.csv';

            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
    }
}
