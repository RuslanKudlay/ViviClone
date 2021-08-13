import { Injectable } from '@angular/core';
import { RequestHandlerHelper } from './request-handler-helper';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ImageService extends RequestHandlerHelper {
    constructor(private http: HttpClient) {
        super();      
    }

    getImage(url) {
       return this.http.get(url);
    }
}
