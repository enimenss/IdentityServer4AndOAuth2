import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ServerHttpInterceptor implements HttpInterceptor {

  constructor(private oidcSecurityService: OidcSecurityService) {}
  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {

    const token = this.oidcSecurityService.getToken();
    if (token) {
          request = request.clone({
            headers: request?.headers.set('Authorization', 'Bearer ' + token),
          });
        }

    return next.handle(request).pipe(
          catchError((httpError: any) => {
            console.log('Handled', httpError);
            return throwError(httpError);
          })
        );
  }
}
