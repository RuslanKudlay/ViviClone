import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AddEditPostComponent } from "./add-edit-post/add-edit-post.component";
import { PostListComponent } from "./post-list/post-list.component";

const routes: Routes = [
    { path: '', component: PostListComponent },
    { path: ':subUrl', component: AddEditPostComponent },
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class PostRoutingModule { }
