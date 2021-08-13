import { NgModule } from "@angular/core";
import { TranslateLoaderHelper } from "../../common/translate-helper.component";
import { SharedModule } from "../../shared.module";
import { AddEditSliderComponent } from "./add-edit-slide/add-edit-slide.component";
import { SliderListComponent } from "./slider-list/slider-list.component";
import { SliderRoutingModule } from "./slider-routing.module";

@NgModule({
    declarations: [
        SliderListComponent, AddEditSliderComponent
    ],
    imports: [
        SliderRoutingModule,
        SharedModule
    ],
    providers: [TranslateLoaderHelper]
})
export class SliderModule { }
