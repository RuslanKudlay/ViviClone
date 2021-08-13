import { OrderHistoryModel } from './order-history.model';
import { OrderDetailsModel } from './order-details.model';
import { UserModel } from './user-model';

export class OrderModel {
    public id: string;
    public orderNumber: string;
    public countOfWares: number;
    public createdDate: Date;
    public status: string;
    public deliveryService: string;
    public declarationNumber: string;
    public orderHistories: OrderHistoryModel[];
    public user: UserModel;
    public orderDetails: OrderDetailsModel[];
}
