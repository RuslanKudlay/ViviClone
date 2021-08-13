import { QueryBaseModel } from './base-query.model';
import { Ware } from '../ware.model';

export class WareQueryModel extends QueryBaseModel<Ware> {
    public NameContains: string = '';
    public PriceContains: string = '';
    public VendoreCodeContains: string = '';
    public GroupOfWareContains: string = '';
}
