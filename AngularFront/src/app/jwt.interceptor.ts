import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
// import { OAuthService } from 'angular-oauth2-oidc';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private router: Router,/* private oauthService: OAuthService*/) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    debugger;
    //const token = this.oauthService.getAccessToken(); // Use OAuthService to get token
    const token = this.getCookie('JwtToken');
    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    }

    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          this.handleUnauthorized();
        }
        return throwError(error);
      })
    );
  }

  private handleUnauthorized() {
   /* this.oauthService.logOut(); // Log out using OAuthService  */
    this.router.navigate(['/login']);
  }

  getCookie(name: string): string | null {
    const nameEQ = name + '=';
    const cookiesArray = document.cookie.split(';');

    for (let i = 0; i < cookiesArray.length; i++) {
      let cookie = cookiesArray[i].trim();

      if (cookie.indexOf(nameEQ) === 0) {
        return cookie.substring(nameEQ.length, cookie.length);
      }
    }

    return null; // 返回 null 如果没有找到该 cookie
  }
}
