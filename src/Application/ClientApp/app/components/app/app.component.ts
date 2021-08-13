import { Component } from '@angular/core';
import { LanguageService } from '../../services/language.service';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    constructor(private languageService: LanguageService) { }

    ngOnInit() {
        this.languageService.SetLanguage();
    }
}
