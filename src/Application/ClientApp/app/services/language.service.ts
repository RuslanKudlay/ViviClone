import { Injectable } from '@angular/core';
import { TranslateLoader, TranslateService } from '@ngx-translate/core';
import 'rxjs/Rx';

@Injectable()
export class LanguageService {
    constructor(private translateLoader: TranslateLoader, private translateService: TranslateService) { }

    public SetLanguage() {
        this.translateLoader.getTranslation('').subscribe(res => {
            this.setTranslation(res, res.lang);
        });
    }

    private setTranslation(data: any, languageCode: any) {
        this.translateService.setTranslation(languageCode, data);
        this.translateService.use(languageCode);
    }
}
