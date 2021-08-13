import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import { Observable } from 'rxjs';
import * as jwt_decode from 'jwt-decode';
import { LoginModel } from '../models/auth/login.model';

@Injectable()
export class AuthService {
  constructor(private httpClient: HttpClient) { }

  public getToken(): string {
    return localStorage.getItem('token');
  }

  public isAuthenticated(): boolean {
    const token = this.getToken();
    return token != null;
  }

  private getPayload(): any {
    const token = this.getToken();
    if (token) {
      const payload: any = jwt_decode(token);
      return payload;
    }
    return null;
  }

  public getUserId(): string {
    const payload: any = this.getPayload();
    if (payload) {
      return payload.userId;
    }
    return null;
  }

  public getRoles(): string[] {
    const payload: any = this.getPayload();
    if (payload) {
      return Array.isArray(payload.roles) ? payload.roles : [payload.roles];
    }
    return null;
  }

  public hasRole(roles: string[]): boolean {
    const userRoles = this.getRoles();
    return userRoles.filter(value => roles.includes(value)).length != 0;
  }

  public login(model: LoginModel): Observable<any> {
    return this.httpClient.post('api/account/login', model);
  }

  public signUp(email: string, companyName: string, password: string): Observable<any> {
    return this.httpClient.post('api/account/signUp', { email, companyName, password });
  }

  public forgotPassword(email: string) {
    return this.httpClient.post('api/account/forgotPassword', { email });
  }

  public completeRegistration(userId: string, code: string, firstName: string, lastName: string): Observable<any> {
    return this.httpClient.post('api/account/completeRegistration', { userId, code, firstName, lastName });
  }

  public resetPassword(userId: string, code: string, newPassword: string, confirmPassword: string): Observable<any> {
    return this.httpClient.post('api/account/resetPassword', { userId, code, newPassword, confirmPassword });
  }

  public logout(): void {
    localStorage.removeItem('token');
  }
}
