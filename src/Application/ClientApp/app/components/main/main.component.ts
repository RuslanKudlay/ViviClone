import { Component } from '@angular/core';
import { LanguageService } from '../../services/language.service';

@Component({
    selector: 'main',
    templateUrl: './main.component.html',
    styleUrls: ['./main.component.css']
})
export class MainComponent {
    constructor(private languageService: LanguageService) { }

    ngOnInit() {
        this.languageService.SetLanguage();
    }
}
