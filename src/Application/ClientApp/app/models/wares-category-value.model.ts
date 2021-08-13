import { Ware } from './ware.model';
import { CategoryValues } from './category-values.model';

export class WaresCategoryValues {
    id: number;   
    ware: Ware = new Ware();   
    categoryValue: Array<CategoryValues> = new Array();
}
