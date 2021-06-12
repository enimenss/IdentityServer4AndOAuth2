import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { environment } from 'projects/application/src/environments/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(private oidcSecurityService: OidcSecurityService) { }

  ngOnInit(): void {
  }

  login() {
    this.oidcSecurityService.authorize();
  }

  isAuth() {
    this.oidcSecurityService.checkAuth().subscribe((auth) => console.log('is authenticated', auth));
  }

  getUserInfo(){
     this.oidcSecurityService.userData$.subscribe((user) => {
      console.log("User -->", user);
    })
  }

  signOut(){
   this.oidcSecurityService.logoffAndRevokeTokens().subscribe();
  }

}
