import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { GroupOfWaresListComponent } from './group-of-wares-list/group-of-wares-list.component';
import { GroupOfWaresRoutingModule } from './group-of-wares-routing.module';
import { GroupOfWaresComponent } from './group-of-wares/group-of-wares.component';

@NgModule({
    declarations: [
        GroupOfWaresComponent,
        GroupOfWaresListComponent
    ],
    imports: [
        GroupOfWaresRoutingModule,
        SharedModule
    ]
})
export class GroupOfWaresModule { }
