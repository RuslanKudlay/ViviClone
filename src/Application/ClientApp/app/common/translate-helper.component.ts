import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateLoader } from '@ngx-translate/core';
import { RequestHandlerHelper } from '../services/request-handler-helper';

@Injectable()
export class TranslateLoaderHelper extends  RequestHandlerHelper implements TranslateLoader  {
    constructor(private http: HttpClient) {
        super();   
    }

    getTranslation(lang: string): any {
        return this.getTranslationFromServer();
    }

    private getTranslationFromServer() {      
        return this.http.get('api/Translation/getTranslate', this.httpOptions);
    }
}
