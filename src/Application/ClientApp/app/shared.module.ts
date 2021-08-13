import { EditAnnouncementButtonComponent } from './extentsions/edit-announcement-button.component';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { AgGridModule } from 'ag-grid-angular/main';
import { DragAndDropModule } from 'angular-draggable-droppable';
import { AngularMultiSelectModule } from 'angular2-multiselect-dropdown';
import { TinymceModule } from 'angular2-tinymce';
import { NgDatepickerModule } from 'ng2-datepicker';
import { ColorPickerModule } from 'ngx-color-picker';
import { AgGridHelperComponent } from './common/ag-grid-helper/ag-grid-helper.component';
import { CategorySelectComponent } from './common/category-select/category-select.component';
import { CategoryValuesSelectModalComponent } from './common/category-values-select-modal/category-values-select-modal.component';
import { FilterCategoryPipe } from './common/filter-category-pipe';
import { TinymceEditorComponent } from './common/tinymce-editor.component';
import { TranslateLoaderHelper } from './common/translate-helper.component';
import { TreeViewGOWSelectComponent } from './common/tree-view-gow-select/tree-view-gow-select.component';
import { TreeViewGOWComponent } from './common/tree-view-gow/tree-view-gow.component';
import { CategoryValuesModalComponent } from './components/category/common/category-values-modal.component';
import { DeleteButtonComponent } from './extentsions/delete-button.component';
import { EditOrderButtonComponent } from './extentsions/edit-order-button.component';
import { EditWareButtonComponent } from './extentsions/edit-ware-button.component';
import { EditUserButtonComponent } from './extentsions/edit-user-button.component';
import { EditBrandButtonComponent } from './extentsions/edit-brand-button.component';
import { EditCategoryButtonComponent } from './extentsions/edit-category-button.component';
import { EditPromotionsButtonComponent } from './extentsions/edit-promotion-button.component';
import { EditPostButtonComponent } from './extentsions/edit-post-button-component';
import { EditSliderButtonComponent } from './extentsions/edit-slider-button.component';

@NgModule({
    declarations: [
        AgGridHelperComponent,
        TinymceEditorComponent,
        TreeViewGOWComponent,
        TreeViewGOWSelectComponent,
        FilterCategoryPipe,
        DeleteButtonComponent,
        EditOrderButtonComponent,
        EditWareButtonComponent,
        EditBrandButtonComponent,
        CategorySelectComponent,
        CategoryValuesSelectModalComponent,
        CategoryValuesModalComponent,
        EditUserButtonComponent,
        EditCategoryButtonComponent,
        EditPromotionsButtonComponent,
        EditAnnouncementButtonComponent,
        EditPostButtonComponent,
        EditSliderButtonComponent
    ],
    imports: [
        AngularMultiSelectModule,
        NgDatepickerModule,
        ColorPickerModule,
        CommonModule,
        RouterModule,
        FormsModule,
        DragAndDropModule,
        TinymceModule.withConfig(),
        AgGridModule.withComponents([]),
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useClass: TranslateLoaderHelper
            }
        }),
      ReactiveFormsModule
    ],
    exports: [
        AngularMultiSelectModule,
        CategorySelectComponent,
        NgDatepickerModule,
        ColorPickerModule,
        CommonModule,
        FormsModule,
        AgGridHelperComponent,
        AgGridModule,
        TinymceEditorComponent,
        TinymceModule,
        TreeViewGOWComponent,
        FilterCategoryPipe,
        TranslateModule,
        TreeViewGOWSelectComponent,
      CategoryValuesModalComponent,
      ReactiveFormsModule
    ],
    providers: [
        TranslateLoaderHelper
    ],
    entryComponents: [
        EditUserButtonComponent,
        EditOrderButtonComponent,
        DeleteButtonComponent,
        EditWareButtonComponent,
        CategoryValuesSelectModalComponent,
        CategoryValuesModalComponent,
        EditBrandButtonComponent,
        EditCategoryButtonComponent,
        EditPromotionsButtonComponent,
        EditAnnouncementButtonComponent,
        EditPostButtonComponent,
        EditSliderButtonComponent
    ]
})
export class SharedModule { }
