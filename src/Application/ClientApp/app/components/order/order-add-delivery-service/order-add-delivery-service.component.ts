import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DeliveryServiceModel } from '../../../models/delivery-service.model';
import { OrderModel } from '../../../models/order.model';
import { OrderService } from '../../../services/order.service';

@Component({
    templateUrl: './order-add-delivery-service.component.html'
})
export class OrderAddDeliveryServiceComponent implements OnInit {

    public order: OrderModel;
    public deliveryServices: DeliveryServiceModel[];
    public loading: boolean = false;

    constructor(private router: Router, private route: ActivatedRoute, private orderService: OrderService) { }

    public ngOnInit(): void {
        this.loading = true;
        this.route.params.subscribe(params => {
            const id = params['id'];
            this.orderService.GetById(id).subscribe((response: OrderModel) => {
                this.order = response;
                this.orderService.getDeliveryServices().subscribe((response) => {
                    this.deliveryServices = response as DeliveryServiceModel[];
                    if (!this.order.deliveryService) {
                        this.order.deliveryService = this.deliveryServices[0].name;
                    }
                    this.loading = false;
                });
            });
        });
    }

    public addDeliveryService(): void {
        this.order.status = 'Sent';
        this.orderService.Update(this.order).subscribe((response: any) => {
            this.router.navigate(['orders']);
        });
    }
}
