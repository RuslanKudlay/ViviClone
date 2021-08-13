import { CategoryValues } from './category-values.model';

export class Category {
    id: number;  
    name: string;
    subUrl: string;
    metaKeywords: string;
    metaDescription: string;
    isEnable: string;
    categoryValues: Array<CategoryValues> = new Array();
}
