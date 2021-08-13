import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { GroupOfWaresListComponent } from "./group-of-wares-list/group-of-wares-list.component";
import { GroupOfWaresComponent } from "./group-of-wares/group-of-wares.component";

const routes: Routes = [
    { path: '', component: GroupOfWaresListComponent },
    { path: ':subUrl', component: GroupOfWaresComponent },
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class GroupOfWaresRoutingModule { }
