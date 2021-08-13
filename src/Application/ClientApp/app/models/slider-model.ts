export enum TypeStatus {
    Main = 1,
    Shop
}

export class SliderModel {
    id: number;
    image: string;  
    linkToWare: string;
    type: TypeStatus;

    base64Image: string;
    location: string;
    nameWare: string;
}