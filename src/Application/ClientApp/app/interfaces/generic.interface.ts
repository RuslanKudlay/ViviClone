export interface GenericMethod {
    GetAll(query: any);
    GetById(id: number);
    GetBySubUrl(subUrl: string);
    Save<T>(model: T);
    Delete(id: number);
    DeleteBySubUrl(subUrl: string);
    Update<T>(model: T);
}
