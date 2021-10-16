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

  subject: any;
  email: string;
  id_token: string

  ngOnInit(): void {
    this.oidcSecurityService.userData$.subscribe((user) => {
      console.log("User -->", user);
      this.subject = user.sub;
      this.email = user.email;
      this.id_token = this.oidcSecurityService.getIdToken();
    })
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
