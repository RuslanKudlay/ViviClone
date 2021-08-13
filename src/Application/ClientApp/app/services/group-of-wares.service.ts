import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Brand } from '../models/brand.model';
import { GroupOfWaresTree } from '../models/group-of-wares-tree-view.model';
import { GroupOfWares } from '../models/group-of-wares.model';
import { GenericRestService } from './generic.service.';

@Injectable()
export class GroupOfWaresService extends GenericRestService<GroupOfWares> {

    constructor(protected http: HttpClient) {
        super(http, 'api/GroupOfWares');   
    }

    getGowsByBrand(): Observable<Array<Brand>> {
        return this.http.get<Array<Brand>>(this.actionUrl + '/all', this.httpOptions);  
    }

    getGowsByWareSubUrl(subUrl: string): Observable<GroupOfWares[]> {
        return this.http.get<GroupOfWares[]>(this.actionUrl + '/GetByWareSubUrl/' + subUrl);
    }

    getGroupsWithoutParent(model?: GroupOfWares): Observable<any> {
        return this.http.post('api/GroupOfWares/GroupsWithoutParent', model, this.httpOptions);
    }

    getGroupsWithoutChilds(model?: GroupOfWares): Observable<any> {
        return this.http.post('api/GroupOfWares/GroupsWithoutChilds', model, this.httpOptions);
    }

    getTreeGOW(): Observable<GroupOfWares[]> {
        return this.http.get<GroupOfWares[]>('api/GroupOfWares/Tree');
    }
  
    InitialTreeViewGOW(gow: GroupOfWares, checkedElement?: any): any {
        const element: GroupOfWaresTree = new GroupOfWaresTree();
        element.id = gow.id;
        element.name = gow.name;
        element.isLeaf = false;    
        element.isChecked = false;
        element.isExpanded = false;
      
        if (checkedElement && element.id == checkedElement.id) {
            element.isChecked = true;
        }

        if (gow.childs) {
            gow.childs.forEach(child => {
                element.childs.push(this.InitialTreeViewGOW(child, checkedElement));
            }); 
        } else {
            element.isLeaf = true;
        }

        if (gow.parent) {
            element.parent = this.InitialTreeViewGOW(gow.parent, checkedElement);
        }
   
        return element;
    }

    ChangeTreeViewGOW (gow: GroupOfWaresTree, checkedElement?: GroupOfWaresTree): any {
        gow.isChecked = false;
        if (checkedElement) {
            if (checkedElement.id == gow.id) {
                gow.isChecked = true;    
            }
        }
        if (gow.childs) {
            gow.childs.forEach(child => {
                const item = this.ChangeTreeViewGOW(child, checkedElement);
            });
        }
        return gow;
    }
}
