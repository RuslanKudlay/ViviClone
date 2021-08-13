export class Announcement {
    id: number;
    isEnable: boolean;
    title: string;
    body: string;
    date: Date;
    lastUpdateDate: Date;
    subUrl: string;
    metaKeywords: string;
    metaDescription: string;

    constructor() {
      this.date = new Date();
      this.lastUpdateDate = new Date();
    }
}
