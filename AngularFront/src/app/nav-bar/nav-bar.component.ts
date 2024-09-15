import { AuthService } from '../auth.service';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css'],
})
export class NavBarComponent {
  keywordForSearch: string = '';
  isLogin = false;

  constructor(
    private authService: AuthService,
    private router: Router /*private oauthService: OAuthService*/
  ) {}

  ngOnInit() {
    this.authService.isLoggedIn.subscribe((isLoggedIn) => {
      this.isLogin = isLoggedIn;
    });
  }

  Logout() {
    this.authService.Logout();
  }
}
