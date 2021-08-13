import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { OrderAddDeliveryServiceComponent } from "./order-add-delivery-service/order-add-delivery-service.component";
import { OrderEditComponent } from "./order-edit/order-edit.component";
import { OrderListComponent } from "./order-list/order-list.component";

const routes: Routes = [
    { path: '', component: OrderListComponent },
    { path: ':id', component: OrderEditComponent },
    { path: 'delivery/:id', component: OrderAddDeliveryServiceComponent },
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class OrderRoutingModule { }