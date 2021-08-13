import { Injectable } from '@angular/core';
import { Category } from '../models/category.model';
import { GenericRestService } from './generic.service.';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class CategoryService extends  GenericRestService<Category> {
  constructor(protected http: HttpClient) {
    super(http, 'api/Category');   
  }
}
