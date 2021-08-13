import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { WaresCategoryValuesListComponent } from './wares-category-values-list/wares-category-values-list.component';
import { WaresCategoryValuesRoutingModule } from './wares-category-values-routing.module';
import { WaresCategoryValuesComponent } from './wares-category-values/wares-category-values.component';

@NgModule({
    declarations: [
        WaresCategoryValuesComponent,
        WaresCategoryValuesListComponent
    ],
    imports: [
        WaresCategoryValuesRoutingModule,
        SharedModule
    ]
})
export class WaresCategoryValuesModule { }
