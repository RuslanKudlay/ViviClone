import { Injectable } from '@angular/core';

@Injectable()
export class CreateCyrillicFriendlySuburlService {

    constructor() { }

    createSuburl(newValue: string): string {
        const a = { '1': 'one ', '2': 'two ', '3': 'three ', '4': 'four ', '5': 'five ', '6': 'six ', '7': 'seven ', '8': 'eight ', '9': 'nine ', '0': 'zero ', 'Ё': 'YO', 'Й': 'I', 'Ц': 'TS', 'У': 'U', 'К': 'K', 'Е': 'E', 'Н': 'N', 'Г': 'G', 'Ш': 'SH', 'Щ': 'SCH', 'З': 'Z', 'Х': 'H', 'Ъ': '\'', 'ё': 'yo', 'й': 'i', 'ц': 'ts', 'у': 'u', 'к': 'k', 'е': 'e', 'н': 'n', 'г': 'g', 'ш': 'sh', 'щ': 'sch', 'з': 'z', 'х': 'h', 'ъ': '\'', 'Ф': 'F', 'Ы': 'I', 'В': 'V', 'А': 'a', 'П': 'P', 'Р': 'R', 'О': 'O', 'Л': 'L', 'Д': 'D', 'Ж': 'ZH', 'Э': 'E', 'ф': 'f', 'ы': 'i', 'в': 'v', 'а': 'a', 'п': 'p', 'р': 'r', 'о': 'o', 'л': 'l', 'д': 'd', 'ж': 'zh', 'э': 'e', 'Я': 'Ya', 'Ч': 'CH', 'С': 'S', 'М': 'M', 'И': 'I', 'Т': 'T', 'Ь': '\'', 'Б': 'B', 'Ю': 'YU', 'я': 'ya', 'ч': 'ch', 'с': 's', 'м': 'm', 'и': 'i', 'т': 't', 'ь': '\'', 'б': 'b', 'ю': 'yu' };

        return newValue.trim().toLowerCase()
            .split('').map(function (char) {
                return a[char] || char;
            }).join('')
            .replace(/\s+/g, '-')                   // Replace spaces with -
            .replace(/[^a-z\u0400-\u04FF-]+/g, '')  // Remove all non-word chars except latin and cyrillic (unicode block range)  
            .replace(/\-\-+/g, '-')                 // Replace multiple - with single -
            .replace(/^-+/, '')                     // Trim - from start of text
            .replace(/-+$/, '');                    // Trim - from end of text 
    }
}
