import { NgModule } from "@angular/core";
import { TranslateLoaderHelper } from "../../common/translate-helper.component";
import { SharedModule } from "../../shared.module";
import { OrderAddDeliveryServiceComponent } from "./order-add-delivery-service/order-add-delivery-service.component";
import { OrderEditComponent } from "./order-edit/order-edit.component";
import { OrderListComponent } from "./order-list/order-list.component";
import { OrderRoutingModule } from "./order-routing.module";

@NgModule({
    declarations: [
        OrderListComponent, OrderEditComponent, OrderAddDeliveryServiceComponent
    ],
    imports: [
        OrderRoutingModule,
        SharedModule
    ],
    providers: [TranslateLoaderHelper]
})
export class OrderModule { }
