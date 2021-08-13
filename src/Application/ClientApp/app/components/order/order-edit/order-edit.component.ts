import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderStatusModel } from '../../../models/order-status.model';
import { OrderModel } from '../../../models/order.model';
import { OrderService } from '../../../services/order.service';

@Component({
    templateUrl: './order-edit.component.html',
    styleUrls: ['./order-edit.component.css']
})
export class OrderEditComponent implements OnInit {

    public order: OrderModel = new OrderModel();
    public orderStatuses: OrderStatusModel[];
    public buttonText: string;
    public originStatus: string;
    public loading: boolean = false;

    constructor(private route: ActivatedRoute, private router: Router, private orderService: OrderService) { }

    public ngOnInit(): void {
        this.loading = true;
        this.route.params.subscribe(params => {
            const id = params['id'];
            this.orderService.GetById(id).subscribe((response: OrderModel) => {
                this.order = response;
                this.orderService.getStatuses().subscribe((response) => {
                    this.orderStatuses = response as OrderStatusModel[];
                    this.changeButtonText();
                    this.originStatus = this.order.status;
                    this.loading = false;
                });
            });
        });
    }

    public update(): void {
        this.orderService.Update(this.order).subscribe((response: any) => {
            if (this.order.status == 'Delivered') {
                this.orderService.sendLetter(this.order).subscribe((data: any) => {
                    this.router.navigate(['orders']);
                });
            } else {
                this.router.navigate(['orders']);
            }
        });
    }

    public nextStatus(): void {
        if (this.order.status == 'Created') {
            this.order.status = 'Paid';
            this.buttonText = 'Send';
        } else if (this.order.status == 'Paid') {
            this.router.navigate(['orders/delivery/' + this.order.id]);
        } else if (this.order.status == 'Sent') {
            this.order.status = 'Delivered';
            this.buttonText = 'Ware has arrived';
            this.orderService.sendLetter(this.order);
        }
    }

    public onSelectUpdated(): void {
        this.changeButtonText();
    }

    private changeButtonText(): void {
        if (this.order.status == 'Created') this.buttonText = 'Pay';
        else if (this.order.status == 'Paid') this.buttonText = 'Send';
        else if (this.order.status == 'Sent') this.buttonText = 'Deliver';
        else if (this.order.status == 'Delivered') this.buttonText = 'Order has arrived'; 
    }
}
