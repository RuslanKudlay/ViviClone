import { QueryBaseModel } from './base-query.model';
import { OrderModel } from '../order.model';

export class OrderQueryModel extends QueryBaseModel<OrderModel> {
    constructor() {
        super();
    }
}
