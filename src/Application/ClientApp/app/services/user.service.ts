import { UserStatus } from './../models/user-model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserModel } from '../models/user-model';
import { GenericRestService } from './generic.service.';
import { Observable } from 'rxjs';

@Injectable()
export class UserService extends GenericRestService<UserModel> {
    constructor(protected http: HttpClient){
        super(http, 'api/User');
    }

    public getRoles(): Observable<{id, name}[]> {
        return this.http.get<{id, name}[]>('api/User/roles', this.httpOptions);  
    }

    public getUser(userId: number): Observable<UserModel> {
        return this.http.get<UserModel>('api/User/' + userId, this.httpOptions);  
    }

    public changeRole(role, userId: number): Observable<any> {
        return this.http.post<any>('api/User/change-role/' + userId, role, this.httpOptions);  
    }

    public changeStatus(status: UserStatus, userId: number): Observable<any> {
        return this.http.post<any>('api/User/' + userId + '/change-status/' + status, null, this.httpOptions);  
    }
} 
