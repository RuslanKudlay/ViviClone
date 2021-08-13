import { Component, EventEmitter, Input, OnInit, Output, OnChanges } from '@angular/core';
import { GroupOfWares } from '../../models/group-of-wares.model';
import { CreateCyrillicFriendlySuburlService } from '../../services/create-cyrillic-friendly-suburl.service';
import { ExtensionModalService } from '../../services/extension-modal-service';
import { Router } from '@angular/router';
import { GroupOfWaresService } from 'ClientApp/app/services/group-of-wares.service';
declare var $: any;

@Component({
    selector: 'tree-view-gow-select',
    templateUrl: './tree-view-gow-select.component.html',
    styleUrls: ['./tree-view-gow-select.component.css']
})
export class TreeViewGOWSelectComponent implements OnInit, OnChanges {

	public isChangeNode: boolean = false;
	public nodeChangeIndex: number = -1;
	private currentNameOfChangingGow = '';
	@Input() groupOfWares: GroupOfWares[];	
	@Input() allGroupOfWares: GroupOfWares[];	
	@Input() selectedGroupOfWares: GroupOfWares[];
	@Output() selectedGroupOfWaresChange: EventEmitter<GroupOfWares[]> = new EventEmitter<GroupOfWares[]>();

    constructor(
		private createCyrillicFriendlySuburlService: CreateCyrillicFriendlySuburlService,
		private modalService: ExtensionModalService,
		private router: Router,
		private groupOfWaresService: GroupOfWaresService
	) { }

    public ngOnInit(): void {
		if (!this.groupOfWares) {
			throw new Error("The 'groupOfWares' is required");
		}
		if (!this.allGroupOfWares) {
			throw new Error("The 'allGroupOfWares' is required");
		}
		if (!this.selectedGroupOfWares) {
			throw new Error("The 'selectedGroupOfWares' is required");
		}
	} 

	public ngOnChanges(): void {
		if (!this.groupOfWares) {
			throw new Error("The 'groupOfWares' is required");
		}
		if (!this.allGroupOfWares) {
			throw new Error("The 'allGroupOfWares' is required");
		}
		if (!this.selectedGroupOfWares) {
			throw new Error("The 'selectedGroupOfWares' is required");
		}
	}  

	public isSelectedGow(id: number): boolean {
		const gow = this.selectedGroupOfWares.find(_ => _.id === id);
		if (gow) {
			return true;
		} else {
			return false;
		}
	}

	public clickOnGow(id: number): void {
		const groupOfWare = this.selectedGroupOfWares.find(_ => _.id === id);
		if (groupOfWare) {
			this.unselectGroupOfWare(groupOfWare);
		} else {
			this.selectGroupOfWare(id);
		}
	}

	private unselectGroupOfWare(groupOfWare: GroupOfWares): void {
		this.selectedGroupOfWares.splice(this.selectedGroupOfWares.indexOf(groupOfWare), 1);
		this.selectedGroupOfWaresChange.emit(this.selectedGroupOfWares);
		this.unselectChildren(groupOfWare);
	}

	private unselectChildren(groupOfWares: GroupOfWares): void {
		groupOfWares.childs.forEach(child => {
			const selectedGroupOfWares = this.selectedGroupOfWares.find(_ => _.id === child.id);
			if (selectedGroupOfWares) {
				this.selectedGroupOfWares.splice(this.selectedGroupOfWares.indexOf(selectedGroupOfWares), 1);
				this.selectedGroupOfWaresChange.emit(this.selectedGroupOfWares);	
			}
			if (child.childs) {
				this.unselectChildren(child);
			}
		});
	}

	private selectGroupOfWare(id: number): void {
		const groupOfWare = this.findInAllGroupOfWares(id);
		this.selectedGroupOfWares.push(groupOfWare);
		this.selectedGroupOfWaresChange.emit(this.selectedGroupOfWares);
		if (groupOfWare.parentId && !this.selectedGroupOfWares.find(_ => _.id == groupOfWare.parentId)) {
			this.selectGroupOfWare(groupOfWare.parentId);
		}
	}

	private findInAllGroupOfWares(id: number): GroupOfWares {
		for (let i = 0; i < this.allGroupOfWares.length; i++) {
			if (this.allGroupOfWares[i].id == id) {
				return this.allGroupOfWares[i];
			}
			const findedGroupOfWares = this.deepSearch(id, this.allGroupOfWares[i]);
			if (findedGroupOfWares) {
				return findedGroupOfWares;
			}
		}
		return null;
	}

	private deepSearch(id: number, groupOfWares: GroupOfWares): GroupOfWares {
		if (groupOfWares.childs) {
			for (let i = 0; i < groupOfWares.childs.length; i++) {
				if (groupOfWares.childs[i].id == id) {
					return groupOfWares.childs[i];
				}
				const findedGroupOfWares = this.deepSearch(id, groupOfWares.childs[i]);
				if (findedGroupOfWares) {
					return findedGroupOfWares;
				}
			}
		}
		return null;
	}
}
