import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AddEditBrandComponent } from "./add-edit-brand/add-edit-brand.component";
import { BrandListComponent } from "./brand-list/brand-list.component";

const routes: Routes = [
    { path: '', component: BrandListComponent },
    { path: ':subUrl', component: AddEditBrandComponent },
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class BrandRoutingModule { }
