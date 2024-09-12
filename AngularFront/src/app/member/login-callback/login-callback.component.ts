// import { Component, OnInit } from '@angular/core';
// import { Router } from '@angular/router';
// import { OAuthService } from 'angular-oauth2-oidc';

// @Component({
//   selector: 'app-login-callback',
//   templateUrl: './login-callback.component.html',
// })
// export class LoginCallbackComponent implements OnInit {
//   constructor(private oauthService: OAuthService, private router: Router) {}

//   ngOnInit(): void {
//     // 尝试处理 OAuth2 回调
//     this.oauthService.loadDiscoveryDocumentAndTryLogin().then(() => {
//       debugger;
//       if (this.oauthService.hasValidAccessToken()) {
//         console.log('Login successful!');
//         // 如果登录成功，跳转到应用的主页面
//         this.router.navigateByUrl('login/mmain');
//       } else {
//         console.error('Login failed or was canceled');
//         // 如果登录失败，跳转到登录页面
//         this.router.navigate(['/login']);
//       }
//     });
//   }
// }
