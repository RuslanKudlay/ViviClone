import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DialogService } from 'ng2-bootstrap-modal';
import { GroupOfWares } from '../../../models/group-of-wares.model';
import { CreateCyrillicFriendlySuburlService } from '../../../services/create-cyrillic-friendly-suburl.service';
import { GroupOfWaresService } from '../../../services/group-of-wares.service';
import { Observable } from 'rxjs';

@Component({
    selector: 'group-of-wares-list',
    templateUrl: './group-of-wares-list.component.html',
    styleUrls: ['./group-of-wares-list.component.css']
})
export class GroupOfWaresListComponent implements OnInit {
    public isEditBrandName: boolean = false;
    public gows: GroupOfWares[];

    ngOnInit(): void {
        this.groupOfWaresService.getTreeGOW().subscribe((response: GroupOfWares[]) => {
            this.gows = response;
        });
    }

    constructor(
        private groupOfWaresService: GroupOfWaresService,
        private dialogService: DialogService,
        private router: Router,
        private createCyrillicFriendlySuburlService: CreateCyrillicFriendlySuburlService) { }


    public addParentGow() {

        let nameGOW = prompt('Введите имя для Группы Товаров', 'New Element');

        if (nameGOW == null) {
            alert('Отмена добавления новой группы товаров');
            return;
        }

        const newGow = new GroupOfWares();
        newGow.name = nameGOW;
        newGow.subUrl = this.createCyrillicFriendlySuburlService.createSuburl(newGow.name);
        // this.gows.push(newGow);
        this.groupOfWaresService.Save(newGow).subscribe((response) => {
            this.gows.push(response);
        }, error => {
            alert(error.error);
        });
    }
}
