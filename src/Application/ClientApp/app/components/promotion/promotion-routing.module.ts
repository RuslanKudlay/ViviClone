import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AddEditPromotionComponent } from "./add-edit-promotion/add-edit-promotion.component";
import { PromotionListComponent } from "./promotion-list/promotion-list.component";

const routes: Routes = [
    { path: '', component: PromotionListComponent },
    { path: ':subUrl', component: AddEditPromotionComponent },
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class PromotionRoutingModule { }
