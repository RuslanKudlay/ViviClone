import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { CategoryListComponent } from './category-list/category-list.component';
import { CategoryRoutingModule } from './category-routing.module';
import { CategoryComponent } from './category/category.component';

@NgModule({
    imports: [
        CategoryRoutingModule,
        SharedModule
    ],
    declarations: [
        CategoryComponent,
        CategoryListComponent
    ]
})
export class CategoryModule { }
