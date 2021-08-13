import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { AddEditPostComponent } from './add-edit-post/add-edit-post.component';
import { PostListComponent } from './post-list/post-list.component';
import { PostRoutingModule } from './post-routing.module';

@NgModule({
    declarations: [
        AddEditPostComponent,
        PostListComponent
    ],
    imports: [
        PostRoutingModule,
        SharedModule
    ]
})
export class PostModule { }
