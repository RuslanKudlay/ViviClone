import { GenericRestService } from './generic.service.';
import { OrderModel } from '../models/order.model';
import { Injectable } from '../../../node_modules/@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrderStatusModel } from '../models/order-status.model';
import { DeliveryServiceModel } from '../models/delivery-service.model';

@Injectable()
export class OrderService extends GenericRestService<OrderModel> {
    constructor(protected http: HttpClient){
        super(http, 'api/Order');
    }

    public getStatuses(): Observable<OrderStatusModel[]> {
        return this.http.get<OrderStatusModel[]>('api/Order/Statuses/all', this.httpOptions);  
    }

    public getDeliveryServices(): Observable<DeliveryServiceModel[]> {
        return this.http.get<DeliveryServiceModel[]>('api/Order/DeliveryServices/all', this.httpOptions);
    }

    // Send an email that the order has arrived.
    public sendLetter(order: OrderModel): Observable<any> {
        return this.http.post<any>('api/Order/Send', order);
    }
} 
