import { QueryBaseModel } from './base-query.model';
import { Brand } from '../brand.model';

export class BrandQueryModel extends QueryBaseModel<Brand> {
    public NameContains: string = '';
    public ShortDescriptionContains: string = '';
    public MetaKeywordsContains: string = '';
}
