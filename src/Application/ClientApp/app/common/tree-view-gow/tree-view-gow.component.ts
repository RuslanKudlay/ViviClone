import { Component, Input, OnInit } from '@angular/core';
import { GroupOfWares } from '../../models/group-of-wares.model';
import { CreateCyrillicFriendlySuburlService } from '../../services/create-cyrillic-friendly-suburl.service';
import { ExtensionModalService } from '../../services/extension-modal-service';
import { Router } from '@angular/router';
import { GroupOfWaresService } from '../../services/group-of-wares.service';
// declare var $: any;
@Component({
	selector: 'tree-view-gow',
	templateUrl: './tree-view-gow.component.html',
	styleUrls: ['./tree-view-gow.component.css']
})
export class TreeViewGOWComponent implements OnInit {

	public isChangeNode: boolean = false;
	public nodeChangeIndex: number = -1;
	@Input() gows: GroupOfWares[];
	@Input() allGows: GroupOfWares[];

	constructor(private createCyrillicFriendlySuburlService: CreateCyrillicFriendlySuburlService,
		private modalService: ExtensionModalService, private router: Router,
		private groupOfWaresService: GroupOfWaresService) { }

	public ngOnInit(): void { }

	public editGroupOfWares(groupOfWares: GroupOfWares) {
		this.router.navigate(['/group-of-wares/' + groupOfWares.subUrl]);
	}

	public addNewNode(index: number, parentId: number) {
		let nameNode = prompt('Введите имя для подкатегории', 'New Element');

		if (nameNode == null) {
			alert('Отмена добавления новой подкатегории');
			return;
		}

		const newGow = new GroupOfWares();
		newGow.name = nameNode;
		newGow.subUrl = this.createCyrillicFriendlySuburlService.createSuburl(newGow.name);
		newGow.parentId = parentId;
		this.groupOfWaresService.Save(newGow).subscribe(response => {
			this.gows[index].childs.push(response);
		}, error => {
            alert(error.error);
        });
	}

	public deleteNode(index: number) {
		this.modalService.showDeleteConfirmModal().subscribe(response => {
			if (response) {
				this.groupOfWaresService.Delete(this.gows[index].id).subscribe(response => {
					this.gows.splice(index, 1);
				});
			}
		});
	}

	public isThirdNode(node: GroupOfWares) {
		if (node.parentId != null) {
			let parentGow = this.allGows.find(x => x.id == node.parentId);
			if (parentGow == undefined) return false;
			else return true;
		}
		else return true;
	}
}
