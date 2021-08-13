import { Observable } from 'rxjs';
import { GenericMethod } from '../interfaces/generic.interface';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';


export abstract class GenericRestService<T> implements GenericMethod {
    protected httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json'
        }),
        params: new HttpParams()
    };  

    constructor(protected http: HttpClient, protected actionUrl: string) { }
  
    get(): Observable<any> {
        return this.http.get<T>(this.actionUrl + '/all', this.httpOptions);  
    }

    GetAll<Q>(query: Q): Observable<any> {
        return this.http.post<T>(this.actionUrl + '/all', query, this.httpOptions);        
    }    
    
    GetById(id: any): Observable<T> {
        return this.http.get<T>(this.actionUrl + '/' + id, this.httpOptions);    
    }
    GetBySubUrl(subUrl: string) {
        return this.http.get<T>(this.actionUrl + '/' + subUrl, this.httpOptions);
    }

    Save<T>(model: T): Observable<T> {
        return this.http.post<T>(this.actionUrl, model, this.httpOptions);   
    }

    Delete(id: number) {
        return this.http.delete(this.actionUrl + '/Delete/' + id, this.httpOptions);
    }
    
    DeleteBySubUrl(subUrl: string) {
        this.AddSubUrlParam(subUrl);
        return this.http.delete(this.actionUrl, this.httpOptions);
    }

    Update<T>(model: T) {
        return this.http.put<T>(this.actionUrl, model, this.httpOptions);
    }   

    private AddSubUrlParam(subUrl: string){
        this.httpOptions.params.append('subUrl', subUrl);         
    }
} 
