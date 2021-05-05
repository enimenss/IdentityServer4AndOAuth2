import { ShellModule } from './shell/shell.module';
import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FlexLayoutModule } from '@angular/flex-layout';
import { environment } from '../environments/environment';
import {  AuthModule, LogLevel,  OidcConfigService, OidcSecurityService } from 'angular-auth-oidc-client';
import { ErrorService } from './shared/services/error.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ServerHttpInterceptor } from './shared/interceptors/server-http.interceptor';
import { Router } from '@angular/router';

export function configureAuth(oidcConfigService: OidcConfigService) {
  return () =>
      oidcConfigService.withConfig({
        stsServer: environment.authorityUrl,
        redirectUrl: window.location.origin,
        postLogoutRedirectUri: window.location.origin,
        clientId: 'AngularPKCE',
        scope: 'openid profile resource.full.access resourceCMS.full.access offline_access',
        responseType: 'code',
        useRefreshToken: true,
        silentRenew: true,
        silentRenewUrl: `${window.location.origin}/silent-renew.html`,
        logLevel: LogLevel.None,
        startCheckSession: true,
        autoUserinfo: true,
        storage: window.localStorage,
        maxIdTokenIatOffsetAllowedInSeconds: 60*60*12, //12h (+/- from utc across the globe)
        renewTimeBeforeTokenExpiresInSeconds: 60*2, //2 mins
        tokenRefreshInSeconds:60*1 //1 mins
      });
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FlexLayoutModule,
    AuthModule.forRoot(),
  ],
  providers: [
    OidcConfigService,
    {
        provide: APP_INITIALIZER,
        useFactory: configureAuth,
        deps: [OidcConfigService],
        multi: true,
    },
    ErrorService,
    { provide: HTTP_INTERCEPTORS,
      useClass: ServerHttpInterceptor,
       multi: true,
       deps: [OidcSecurityService]
    },
   ],
  bootstrap: [AppComponent]
})
export class AppModule { }
