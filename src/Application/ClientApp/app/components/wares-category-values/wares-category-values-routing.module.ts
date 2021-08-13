import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { WaresCategoryValuesListComponent } from "./wares-category-values-list/wares-category-values-list.component";
import { WaresCategoryValuesComponent } from "./wares-category-values/wares-category-values.component";

const routes: Routes = [
    { path: '', component: WaresCategoryValuesListComponent },
    { path: ':subUrl', component: WaresCategoryValuesComponent },
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class WaresCategoryValuesRoutingModule { }
