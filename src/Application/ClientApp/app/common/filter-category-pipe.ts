import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
  name: 'filterCategory'
})
export class FilterCategoryPipe implements PipeTransform {
  transform(items: any[], searchText: string): any[] {
    if (!items) return [];
    if (!searchText) return items;
      searchText = searchText.toLowerCase();
      return items.filter(it => {
          return it.category.name.toLowerCase().includes(searchText);
      });
  }
}
