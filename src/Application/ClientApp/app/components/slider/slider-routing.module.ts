import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AddEditSliderComponent } from "./add-edit-slide/add-edit-slide.component";
import { SliderListComponent } from "./slider-list/slider-list.component";

const routes: Routes = [
    { path: '', component: SliderListComponent },
    { path: ':subUrl', component: AddEditSliderComponent },
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class SliderRoutingModule { }