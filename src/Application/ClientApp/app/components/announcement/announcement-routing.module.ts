import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AddEditAnnouncementComponent } from "./add-edit-announcement/add-edit-announcement.component";
import { AnnouncementListComponent } from "./announcement-list/announcement-list.component";

const routes: Routes = [
    { path: '', component: AnnouncementListComponent },
    { path: ':subUrl', component: AddEditAnnouncementComponent },
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class AnnouncementRoutingModule { }
