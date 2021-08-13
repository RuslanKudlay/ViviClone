import { Promotion } from './../promotion.model';
import { QueryBaseModel } from './base-query.model';

export class PromotionQueryModel extends QueryBaseModel<Promotion> {
    public TitleContains: string = '';
    public BodyContains: string = '';
}
