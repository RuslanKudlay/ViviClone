import { Category } from './../category.model';
import { QueryBaseModel } from './base-query.model';

export class CategoryQueryModel extends QueryBaseModel<Category> {
    public NameContains: string = '';
}
