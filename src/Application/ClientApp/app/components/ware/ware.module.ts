import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { AddEditWareComponent } from './add-edit-ware/add-edit-ware.component';
import { WareImportComponent } from './ware-import/ware-import.component';
import { WareListComponent } from './ware-list/ware-list.component';
import { WareRoutingModule } from './ware-routing.module';

@NgModule({
    declarations: [
        AddEditWareComponent,
        WareListComponent,
        WareImportComponent
    ],
    imports: [
        WareRoutingModule,
        SharedModule
    ]
})
export class WareModule { }
