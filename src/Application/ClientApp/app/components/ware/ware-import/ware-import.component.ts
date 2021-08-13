import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Papa } from 'ngx-papaparse';
import { WareQueryModel } from '../../../models/query-models/ware-query.model';
import { Ware } from '../../../models/ware.model';
import { WareService } from '../../../services/ware.service';

@Component({
    templateUrl: './ware-import.component.html',
    styleUrls: ['./ware-import.component.css']
})
export class WareImportComponent {
    public importedWares: Ware[];
    private originWares: Ware[];

    constructor(
        private wareService: WareService,
        private papa: Papa, private router: Router) {
                this.wareService.GetAll(new WareQueryModel()).subscribe(data => {
                this.originWares = data.result;
                this.importedWares = this.wareService.getWaresForImport();
            })
    }

    public save(): void {
        this.wareService.GetAll(new WareQueryModel()).subscribe((data: any) => {
            const loadedWares: Ware[] = data.result;
            for (let i = 0; i < this.importedWares.length; i++) {
                const ware: Ware = loadedWares.find(w => w.id == this.importedWares[i].id);
                if (ware && this.isChanged(ware)) {
                    this.wareService.Update(this.importedWares[i]).subscribe();
                } else {
                    this.wareService.Save(this.importedWares[i]).subscribe();
                }
            }
            this.router.navigate(['/wares']);
        });
    }

    public cancel(): void {
        this.router.navigate(['/wares']);
    }

    public showNames(values) {
        let res = '';
        values ? values.forEach(category => {
            res += category.name + ' ';
        }) : [];
        return res;
    }
    public isChanged(importedWare: Ware): boolean {
        const ware: Ware = this.originWares.find(w => w.id == importedWare.id);

        if (ware) {
            // We need the next record, because we get the null value from the server, and when reading the csv an empty string.
            // These objects are not equal when comparing and we have two different objects, although they must be the same.
            // That is, for string objects, we do an additional check to avoid this bug.
            if (ware.averageRate != importedWare.averageRate || ware.brandId != importedWare.brandId || ware.isEnable != importedWare.isEnable || ware.price != importedWare.price) {
                return true;
            }
            if (ware.name && importedWare.name && ware.name != importedWare.name || ware.base64Image && importedWare.base64Image && ware.base64Image != importedWare.base64Image
                || ware.brandName && importedWare.brandName && ware.brandName != importedWare.brandName
                || ware.metaKeywords && importedWare.metaKeywords && ware.metaKeywords != importedWare.metaKeywords || ware.metaDescription && importedWare.metaDescription && ware.metaDescription != importedWare.metaDescription
                || ware.subUrl && importedWare.subUrl && ware.subUrl != importedWare.subUrl || ware.text && importedWare.text && ware.text != importedWare.text
                || ware.vendorCode && importedWare.vendorCode && ware.vendorCode != importedWare.vendorCode || ware.wareImage && importedWare.wareImage && ware.wareImage != importedWare.wareImage) {
                return true;
            }
        }

        return false;
    }

    public isAdded(importedWare: Ware): boolean {
        const ware: Ware = this.originWares.find(w => w.id == importedWare.id);

        if (ware) {
            return false;
        }

        return true;
    }
}
