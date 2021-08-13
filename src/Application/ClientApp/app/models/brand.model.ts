import { GroupOfWares } from './group-of-wares.model';

export class Brand {
    id: number;
    isEnable: boolean;
    position: number;
    color: string;
    colorTitle: string;
    logoImage: string;
    base64Image: string;
    name: string;
    shortDescription: string;
    body: string;
    subUrl: string;
    metaKeywords: string;
    metaDescription: string;
    gows: Array<GroupOfWares>;
}
