export class GroupOfWares {
    id: number;
    isEnable: boolean;
    name: string;    
    parent: GroupOfWares;
    parentId?: number;
    subUrl: string;
    metaKeywords: string;
    metaDescription: string;  
    shortDescription: string;
    logoImage: string;
    base64Image: string;
    childs: Array<GroupOfWares> = new Array<GroupOfWares>();
    iconString: string;
}
