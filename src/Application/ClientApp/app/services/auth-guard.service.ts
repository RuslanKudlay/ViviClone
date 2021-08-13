import {Injectable} from '@angular/core';
import {CanActivate, Router, CanActivateChild} from '@angular/router';
import {AuthService} from './auth.service';

@Injectable()
export class AuthGuardService implements CanActivate, CanActivateChild {
  
  constructor(
    private auth: AuthService,
    private router: Router
  ) {
  }

  public canActivate(): boolean {
    if (!this.auth.isAuthenticated()) {
      this.router.navigate(['login'], {
        replaceUrl: true,
        queryParams: {returnUrl: window.location.pathname}
      });
      return false;
    }
    return true;
  }

  public canActivateChild(): boolean {
    if (!this.auth.isAuthenticated()) {
      this.router.navigate(['login'], {
        replaceUrl: true,
        queryParams: {returnUrl: window.location.pathname}
      });
      return false;
    }
    return true;  
  } 
}
