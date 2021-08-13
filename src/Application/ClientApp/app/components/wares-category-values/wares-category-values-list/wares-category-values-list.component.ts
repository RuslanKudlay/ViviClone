import { Component } from '@angular/core';
import { WaresCategoryValues } from '../../../models/wares-category-value.model';
import { WaresCategoryValuesService } from '../../../services/wares-category-values.service';

@Component({
    templateUrl: './wares-category-values-list.component.html'
})
export class WaresCategoryValuesListComponent {
  waresCategoryValues: Array<WaresCategoryValues> = new Array<WaresCategoryValues>();
  status: string;

  constructor(private waresCategoryValuesService: WaresCategoryValuesService) {    
    this.waresCategoryValuesService.getAll().subscribe((result) => {
      this.waresCategoryValues = result;
    });
  }

  delete(id: number, index: number) {
    this.waresCategoryValuesService.delete(id).subscribe((result) => {
      this.waresCategoryValues.splice(index, 1);
      this.status = 'Deleted';
    });
  }
}
