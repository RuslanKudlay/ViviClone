import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'ClientApp/environments/environment';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';

@Injectable()
export class Interceptor implements HttpInterceptor {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {

  }

  intercept(request: HttpRequest<any>, next: HttpHandler) : Observable<HttpEvent<any>> {
    request = request.clone({setHeaders: { 'Content-Type': 'application/json' }});
    if (!request.url.startsWith(environment.apiUrl)) {
      request = request.clone({ 
          url: environment.apiUrl + request.url 
        });
    }
    if (this.authService.isAuthenticated()) {
      request = request.clone({
        headers: request.headers.set('Authorization', 'Bearer ' + this.authService.getToken())
      });
    }
    // return next.handle(request);
    return next.handle(request).pipe(
      tap(() => {}, (err: any) => {
      if (err instanceof HttpErrorResponse) {
        if (err.status === 401) {
          this.router.navigate(['login'], {
            replaceUrl: true,
            queryParams: {returnUrl: window.location.pathname}
          });
        }
      }
    }));
  }
}
