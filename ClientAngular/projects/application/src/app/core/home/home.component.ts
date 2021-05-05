import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { environment } from 'projects/core-ui/src/environments/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(private oidcSecurityService: OidcSecurityService,private httpClient: HttpClient) { }

  ngOnInit(): void {
    // this.httpClient.get(environment.coreApiUrl + '/api/home/index').subscribe();
  }


  signOut(){
   this.oidcSecurityService.logoffAndRevokeTokens().subscribe();
   return false;
  }

}
