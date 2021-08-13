import { UserStatus } from '../models/user-model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SliderModel } from '../models/slider-model';
import { GenericRestService } from './generic.service.';
import { Observable } from 'rxjs';

@Injectable()
export class SliderService extends GenericRestService<SliderModel> {
    constructor(protected http: HttpClient) {
        super(http, 'api/Slider');
    }

    public getSlide(slideId: number): Observable<SliderModel> {
        return this.http.get<SliderModel>('api/Slider/' + slideId, this.httpOptions);
    }
} 
