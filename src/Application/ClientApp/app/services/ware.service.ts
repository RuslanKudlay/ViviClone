import { Injectable } from '@angular/core';
import { Ware } from '../models/ware.model';
import { Observable } from '../../../node_modules/rxjs/Observable';
import { GenericRestService } from './generic.service.';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class WareService extends  GenericRestService<Ware> {
    private waresForImport: Ware[];

    constructor(protected http: HttpClient) {
        super(http, 'api/Ware');   
    } 

    getByPagination(paginationMode: any): Observable<any> {
    return this.http.post('api/Ware/GetWaresByPaging', paginationMode, this.httpOptions);
    };

    delete(id: any): Observable<any> {
        return this.http.delete('api/Ware/Delete/' + id);
    }

    setWaresForImport(wares: Ware[]) {
        this.waresForImport = wares;
    }

    getWaresForImport(): Ware[] {
        return this.waresForImport;
    }
}
