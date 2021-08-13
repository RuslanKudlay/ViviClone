import { GroupOfWares } from './group-of-wares.model';
import { CategoryValues } from './category-values.model';

export class Ware {
    id: number;
    isEnable: boolean = true;
    isBestseller: boolean = false;
    isOnlyForProfessionals: boolean = false;
    vendorCode: string;
    wareImage: string;
    base64Image: string;
    name: string;
    text: string;
    price: string;
    subUrl: string;
    metaKeywords: string;
    metaDescription: string;
    gows: Array<GroupOfWares> = new Array<GroupOfWares>();
    categoryValues: Array<CategoryValues> = new Array<CategoryValues>();
    averageRate: number;
    brandId: number;
    brandName: string;
}
