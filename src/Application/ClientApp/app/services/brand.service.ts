import { Injectable } from '@angular/core';
import { Brand } from '../models/brand.model';
import { GenericRestService } from './generic.service.';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class BrandService extends  GenericRestService<Brand> {
    constructor(protected http: HttpClient) {
        super(http, 'api/Brand');
    }
}
