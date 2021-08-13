import { Injectable } from '@angular/core';
import { GenericRestService } from './generic.service.';
import { Promotion } from '../models/promotion.model';
import { HttpClient } from '@angular/common/http';


@Injectable()
export class PromotionService extends GenericRestService<Promotion> { 
  constructor(protected http: HttpClient) {
    super(http, 'api/Promotion');  
  }
}
