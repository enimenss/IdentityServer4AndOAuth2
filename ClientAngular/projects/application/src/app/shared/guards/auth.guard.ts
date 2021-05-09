import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  
  constructor(private oidcSecurityService: OidcSecurityService){
  }

  async canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Promise<boolean | UrlTree> {
    return await this.isAuth();
  }

  async isAuth(): Promise<boolean> {
    let isAuth = false;
    await this.oidcSecurityService.checkAuth().toPromise().then((auth) => {
      if (auth) {
        isAuth = true;
        // Single logout , timeout 3s to be shure that cookie whas deleted
        // This check combination of session state in storage and idsrv.session cookie 
        this.oidcSecurityService.checkSessionChanged$.subscribe((isChanged) => {
          if (isChanged){
            // setTimeout( () => {
            this.oidcSecurityService.authorize();
            // }, 3000);
          }
        });

      }
      else {
        this.oidcSecurityService.authorize();
      }
    });

    return isAuth;
  }
}
