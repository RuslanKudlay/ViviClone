import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AddEditWareComponent } from "./add-edit-ware/add-edit-ware.component";
import { WareImportComponent } from "./ware-import/ware-import.component";
import { WareListComponent } from "./ware-list/ware-list.component";

const routes: Routes = [
    { path: '', component: WareListComponent },
    { path: 'import', component: WareImportComponent },
    { path: ':subUrl', component: AddEditWareComponent }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class WareRoutingModule { }
