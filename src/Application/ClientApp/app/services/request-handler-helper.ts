import { HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import 'rxjs/Rx';

@Injectable()
export class RequestHandlerHelper {
  protected  httpOptions = {
    headers: new HttpHeaders({
    'Content-Type': 'application/json'
    }),
    params: new HttpParams()
  };  
}
