import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { AddEditBrandComponent } from './add-edit-brand/add-edit-brand.component';
import { BrandListComponent } from './brand-list/brand-list.component';
import { BrandRoutingModule } from './brand-routing.module';

@NgModule({
    declarations: [
        AddEditBrandComponent,
        BrandListComponent
    ],
    imports: [
        BrandRoutingModule,
        SharedModule
    ]
})
export class BrandModule { }
