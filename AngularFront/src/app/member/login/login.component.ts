import { Component, AfterViewInit } from '@angular/core';
import { MemberService } from './../member.service';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { environment } from './../../../environments/environment';

declare var grecaptcha: any;



@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [MemberService]
})
export class LoginComponent implements AfterViewInit {
  email: string = 'jarry6304@hotmail.com';
  pwd: string = 'password1';
  siteKey: string = '6Lf8GToqAAAAAMLRKwyKmVEiMtYeMDqK61sPxWPS';
  googleClientId = environment.googleClientId;



  constructor(private memberService: MemberService, private router: Router, private oauthService: OAuthService) {
    console.log('Google Client ID:', this.googleClientId);
  }

  login() {
    debugger;
    console.log("Login()")
    this.oauthService.initLoginFlow();
  }

  ngAfterViewInit() {
    if (typeof grecaptcha === 'undefined') {
      // 動態加載 reCAPTCHA 腳本
      const script = document.createElement('script');
      script.src = 'https://www.google.com/recaptcha/api.js';
      script.onload = () => {

          grecaptcha.render('recaptcha-container', {
            sitekey: this.siteKey
          });

      };
      document.body.appendChild(script);
    } else {
      grecaptcha.render('recaptcha-container', {
        sitekey: this.siteKey
      });
    }
  }
  onLogin() {
    debugger;
    const recaptchaResponse = grecaptcha.getResponse();
    if (!recaptchaResponse) {
      alert('請進行圖形認證');
      window.location.reload();
      return;
    }

    this.memberService.login(this.email, this.pwd, recaptchaResponse).subscribe({
      next: (response) => {
        if (response.hasAlertMsg) {
          alert(response.alertMsg);
        }
        if (response.isSuccess) {
          this.setCookie('JwtToken', response.data, 1);
          this.router.navigateByUrl('login/mmain');
        }
      },
      error: (error) => {
        console.error('Login error:', error);
        alert('登入失敗');
        // 刷新頁面
        window.location.reload();
      }
    });
  }

  onForgetpwd() {
    console.log('Email:', this.email);

    this.memberService.forgetpwd(this.email).subscribe({
      next: (response) => {
        if (response.hasAlertMsg) {
          alert(response.alertMsg);
        }
      },
      error: (error) => {
        console.error('Email error:', error);
        alert('請輸入正確帳號');
      }
    });
  }

  onregister() {
    this.router.navigateByUrl('login/register');
  }

  setCookie(name: string, value: string, days: number) {
    const expires = new Date();
    expires.setTime(expires.getTime() + (days * 24 * 60 * 60 * 1000));
    const expiresString = 'expires=' + expires.toUTCString();
    document.cookie = `${name}=${value}; ${expiresString}; path=/`;
  }
}
