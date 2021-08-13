import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GroupOfWares } from '../../../models/group-of-wares.model';
import { CreateCyrillicFriendlySuburlService } from '../../../services/create-cyrillic-friendly-suburl.service';
import { ExtensionModalService } from '../../../services/extension-modal-service';
import { GroupOfWaresService } from '../../../services/group-of-wares.service';
import { ImageService } from '../../../services/image.service';
import { UploadImageModalService } from '../../../services/upload-image-modal.service';

@Component({
    templateUrl: './group-of-wares.component.html'
})
export class GroupOfWaresComponent implements OnInit {
    public groupOfWare: GroupOfWares = new GroupOfWares();
    public title: string;
    public subUrl: string;

    // DropDown Settings
    public gows: GroupOfWares[];
    public dropdownListGows: any = [];
    public selectedParent: any;
    public selectedChilds: any;
    public dropdownChildSettings;
    public dropdownParentSettings;

    constructor(
        private groupOfWaresService: GroupOfWaresService, 
        private route: ActivatedRoute,
        private router: Router, 
        private location: Location,
        private uploadImageModalService: UploadImageModalService,
        private imageService: ImageService,
        private createCyrillicFriendlySuburlService: CreateCyrillicFriendlySuburlService,
        private modalService: ExtensionModalService) { }

    public ngOnInit() {
        this.route.params.subscribe(params => {
            this.subUrl = params['subUrl']; 
            this.title = 'Add Group of Ware';    

            if (this.subUrl && this.subUrl != 'newGroupOfWare') {
                this.title = 'Update Group of Ware';
                this.groupOfWaresService.get()
                    .subscribe((response: GroupOfWares[]) => {
                        this.gows = response;
                        // For angular2-multiselect we must have object such as { id: 0, itemName: 'someName'}
                        for (let i = 0; i < this.gows.length; i++) {
                            this.dropdownListGows.push({
                                id: i,
                                itemName: this.gows[i].name,
                                gowId: this.gows[i].id,
                            });
                        }

                        this.groupOfWaresService.GetBySubUrl(this.subUrl)
                            .subscribe((response: GroupOfWares) => {
                                this.groupOfWare = response;

                                this.imageService.getImage(response.logoImage).subscribe((response) => {
                                    this.groupOfWare.base64Image = response as string;
                                });

                                if (this.groupOfWare.parent) {
                                    const selectedParentItem = this.dropdownListGows.find(gow => gow.gowId == this.groupOfWare.parent.id);
                                    this.selectedParent = selectedParentItem;
                                }

                                if (this.groupOfWare.childs) {
                                    const selectedChildItems = this.dropdownListGows.find(gow => this.groupOfWare.childs.some(child => child.id = gow.gowId));
                                    this.selectedChilds = selectedChildItems;
                                }
                            });
                    });
            } 
        });        

        this.dropdownParentSettings = {
            singleSelection: true,
            text: 'Select Parent',
            enableSearchFilter: true,
            classes: 'myclass custom-class',
        };  

        this.dropdownChildSettings = {           
            text: 'Select Child',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            enableSearchFilter: true,
            classes: 'myclass custom-class',
        };   
    }

    addGroupOfWares() {    
        this.groupOfWaresService.Save(this.groupOfWare).subscribe((response: any) => {
            this.location.back();           
        });
    }

    updateGroupOfWares() {
        this.groupOfWaresService.Update(this.groupOfWare).subscribe((response: any) => {
            this.location.back();           
        }); 
    }

    showModalImageUploader() {     
        this.uploadImageModalService.showModal().subscribe((link) => {
            if (link) {
                this.groupOfWare.logoImage = link;
            }
        });
    }   

    onKeyBrandName(newValue: string) {
        this.groupOfWare.name = newValue;
        this.groupOfWare.subUrl = this.createCyrillicFriendlySuburlService.createSuburl(newValue);
    }

    showIconModal() {
        this.modalService.showIconModal().subscribe(result => {
            if (result) {
                this.groupOfWare.iconString = result;
            }
        });
    }
}
