import { NgModule } from "@angular/core";
import { TranslateLoaderHelper } from "../../common/translate-helper.component";
import { SharedModule } from "../../shared.module";
import { UserDetailsComponent } from "./user-details/user-details.component";
import { UserListComponent } from "./user-list/user-list.component";
import { UserRoutingModule } from "./user-routing.module";

@NgModule({
    declarations: [
        UserListComponent, UserDetailsComponent
    ],
    imports: [
        UserRoutingModule,
        SharedModule
    ],
    providers: [TranslateLoaderHelper]
})
export class UserModule { }
