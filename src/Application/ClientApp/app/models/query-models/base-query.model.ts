export class QueryBaseModel<T> {
    public skip: number;
    public take: number;
    public total: number;
    public orderBy: string;
    public orderByDesc: string;
    public dateCreatedFrom: Date;
    public dateCreatedTo: Date;   
    public result: T[];
}
