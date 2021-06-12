import { ShellModule } from './shell/shell.module';
import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FlexLayoutModule } from '@angular/flex-layout';
import { environment } from '../environments/environment';
import { ErrorService } from './shared/services/error.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ServerHttpInterceptor } from './shared/interceptors/server-http.interceptor';
import { Router } from '@angular/router';

import {  AuthModule, LogLevel,  OidcConfigService, OidcSecurityService } from 'angular-auth-oidc-client';

export function configureAuth(oidcConfigService: OidcConfigService) {
  return () =>
      oidcConfigService.withConfig({
        stsServer: 'https://localhost:44333',
        redirectUrl: window.location.origin,
        clientId: 'ClientAngular',
        scope: 'openid email',
        responseType: 'code',
        logLevel: LogLevel.None,
        autoUserinfo: true,
        storage: window.localStorage,       
        maxIdTokenIatOffsetAllowedInSeconds: 60*60*12,
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
