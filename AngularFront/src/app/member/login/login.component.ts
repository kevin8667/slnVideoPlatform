import { AuthService } from '../../auth.service';
import { Component, AfterViewInit, OnInit } from '@angular/core';
import { MemberService } from './../member.service';

import { Router, ActivatedRoute } from '@angular/router';
// import { OAuthService } from 'angular-oauth2-oidc';
import { environment } from './../../../environments/environment';

declare var grecaptcha: any;
interface demo {
  name: string;
}

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [MemberService],
})
export class LoginComponent implements AfterViewInit, OnInit {
  email: string = '';
  pwd: string = '';
  siteKey: string = '6Lf8GToqAAAAAMLRKwyKmVEiMtYeMDqK61sPxWPS';
  googleClientId = environment.googleClientId;
  recaptchaVisible: boolean = true;

  demos: demo[] | undefined;

  selectdemo: demo | undefined;

  ngOnInit() {
    this.demos = [
      { name: '葉仲仁' },
      { name: '江睿庭' },
      { name: '詹喬琳' },
      { name: '林韋廷' },
    ];
  }

  constructor(
    private memberService: MemberService,
    private authService: AuthService,
    private router: Router /*private oauthService: OAuthService*/,
    private route: ActivatedRoute
  ) {
    console.log('Google Client ID:', this.googleClientId);
  }

  LineLogin() {
    this.authService.loginWithLine(false);
  }
  ngAfterViewInit() {
    if (typeof grecaptcha === 'undefined') {
      // 動態加載 reCAPTCHA 腳本
      const script = document.createElement('script');
      script.src = 'https://www.google.com/recaptcha/api.js';
      script.onload = () => {
        grecaptcha.render('recaptcha-container', {
          sitekey: this.siteKey,
        });
      };
      document.body.appendChild(script);
    } else {
      grecaptcha.render('recaptcha-container', {
        sitekey: this.siteKey,
      });
    }
  }
  ondemochange(selectdemo: demo) {
    switch (selectdemo.name) {
      case '葉仲仁':
        this.demo1();
        break;
      case '江睿庭':
        this.demo2();
        break;
      case '詹喬琳':
        this.demo5();
        break;
      case '林韋廷':
        this.demo10();
        break;
      default:
        break;
    }
  }
  onLogin() {
    const recaptchaResponse = grecaptcha.getResponse();
    if (!recaptchaResponse) {
      alert('請進行圖形認證');
      window.location.reload();
      return;
    } else {
      this.loginByPwd();
    }
  }

  loginByPwd() {
    this.memberService.login(this.email, this.pwd).subscribe({
      next: (response) => {
        if (response.hasAlertMsg) {
          alert(response.alertMsg);
        }
        if (response.isSuccess) {
          this.setCookie('JwtToken', response.data.jwtToken, 1);
          this.authService.SetLoginValue();
          this.authService.SetMemberData(response.data);
          const returnUrl =
            this.route.snapshot.queryParamMap.get('returnUrl') || '/';
          this.router.navigateByUrl(returnUrl);
        }
      },
      error: (error) => {
        console.error('Login error:', error);
        alert('登入失敗');
        // 刷新頁面
        window.location.reload();
      },
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
      },
    });
  }

  onregister() {
    this.router.navigateByUrl('login/register');
  }

  setCookie(name: string, value: string, days: number) {
    const expires = new Date();
    expires.setTime(expires.getTime() + days * 24 * 60 * 60 * 1000);
    const expiresString = 'expires=' + expires.toUTCString();
    document.cookie = `${name}=${value}; ${expiresString}; path=/`;
  }

  toggleRecaptcha() {
    this.recaptchaVisible = !this.recaptchaVisible;
    const reCaptchaContainer = document.getElementById('recaptcha-container');
    if (reCaptchaContainer) {
      reCaptchaContainer.style.display = this.recaptchaVisible
        ? 'block'
        : 'none';
    }
  }

  demo1() {
    this.email = 'example1@example.com';
    this.pwd = 'password1';
    this.loginByPwd();
  }

  demo2() {
    this.email = 'example2@example.com';
    this.pwd = 'password2';
    this.loginByPwd();

  }

  demo5() {
    this.email = 'example5@example.com';
    this.pwd = 'password5';
    this.loginByPwd();

  }

  demo10() {
    this.email = 'example10@example.com';
    this.pwd = 'password10';
    this.loginByPwd();

  }
}
