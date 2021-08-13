import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { AddEditPromotionComponent } from './add-edit-promotion/add-edit-promotion.component';
import { PromotionListComponent } from './promotion-list/promotion-list.component';
import { PromotionRoutingModule } from './promotion-routing.module';

@NgModule({
    declarations: [
        AddEditPromotionComponent,
        PromotionListComponent
    ],
    imports: [
        PromotionRoutingModule,
        SharedModule
    ]
})
export class PromotionModule { }
