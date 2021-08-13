import { Injectable } from '@angular/core';
import { CategoryValues } from '../models/category-values.model';
import { GenericRestService } from './generic.service.';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class CategoryValuesService extends  GenericRestService<CategoryValues> {
    constructor(protected http: HttpClient) {
        super(http, 'api/CategoryValues');   
    } 
}
