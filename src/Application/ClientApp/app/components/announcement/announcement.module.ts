import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { AnnouncementListComponent } from './announcement-list/announcement-list.component';
import { AnnouncementRoutingModule } from './announcement-routing.module';
import { AddEditAnnouncementComponent } from './add-edit-announcement/add-edit-announcement.component';

@NgModule({
    declarations: [
        AddEditAnnouncementComponent,
        AnnouncementListComponent
    ],
    imports: [
        AnnouncementRoutingModule,
        SharedModule
    ]
})
export class AnnouncementModule { }
