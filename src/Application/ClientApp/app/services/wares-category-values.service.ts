import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import 'rxjs/Rx';
import { WaresCategoryValues } from '../models/wares-category-value.model';
import { RequestHandlerHelper } from './request-handler-helper';

@Injectable()
export class WaresCategoryValuesService extends  RequestHandlerHelper {
  
    constructor(private http: HttpClient) {
        super();
    }

    add(item: WaresCategoryValues): Observable<any> {
        return this.http.post('api/WaresCategoryValue/Add', item, this.httpOptions);
    }

    getAll(): Observable<any> {
        return this.http.get('api/WaresCategoryValue');
    };

    getById(id: number): Observable<any> {
        return this.http.get('api/WaresCategoryValue/' + id);
    };

    update(item: WaresCategoryValues): Observable<any> {
        return this.http.post('api/WaresCategoryValue/Update', item, this.httpOptions);
    };

    delete(id: number): Observable<any> {
        return this.http.delete('api/WaresCategoryValue/' + id);
    };
}
