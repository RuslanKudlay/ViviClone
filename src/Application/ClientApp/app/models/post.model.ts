export class PostModel {
    id: number;
    isEnable: boolean;
    title: string;
    body: string;
    dateCreated: Date;
    dateModificated: Date;
    subUrl: string;
    metaKeywords: string;
    metaDescription: string;
    imageURL: string;
    base64Image: string;

    constructor() {
        this.dateCreated = new Date();
        this.dateModificated = new Date();
    }
}
