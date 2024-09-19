import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthService } from './../auth.service';

@Component({
  selector: 'app-auth-callback',
  template: `<p>Processing...</p>`,
  styles: []
})
export class AuthCallbackComponent implements OnInit {

  constructor(private route: ActivatedRoute, private http: HttpClient, private router: Router,private authService:AuthService) { }

  ngOnInit(): void {
    debugger;
    // Extract authorization code from query parameters
    this.route.queryParams.subscribe(params => {
      debugger;
      const fullUrl = window.location.href;

      // Extract the code parameter from the URL
      const code = this.extractCodeFromUrl(fullUrl,'code');
      debugger;
      if (code) {
        this.exchangeCodeForToken(code);
      } else {
        // Handle error or redirect if code is not present
        console.error('Authorization code not found');
        //this.router.navigate(['/login']); // Redirect to login page or error page
      }
    });
  }

  private extractCodeFromUrl(url: string,name:string): string | null {
    try {
      // Create a URL object
      const urlObj = new URL(url);

      // Use URLSearchParams to get the code parameter
      const searchParams = new URLSearchParams(urlObj.search);

      // Return the value of the 'code' parameter
      return searchParams.get(name);
    } catch (error) {
      console.error('Error extracting code from URL', error);
      return null;
    }
  }

  // Exchange authorization code for an access token
  private exchangeCodeForToken(code: string): void {
    debugger;
var url="https://localhost:7193/api/auth/callback";
if(this.authService.getCookie("Binding")=="Y")
  url="https://localhost:7193/api/auth/BindingLine";
    this.http.post(url, { code }).subscribe({
      next: (response:any) => {
        if (response.hasAlertMsg) {
          alert(response.alertMsg);

        }
        if (response.isSuccess) {
          if(response.data!="Binding"){
          this.setCookie('JwtToken', response.data.jwtToken, 1);

          this.authService.SetLoginValue();
          this.authService.SetMemberData(response.data);

          }
          this.router.navigateByUrl('login/mmain');
        }else{
          this.router.navigateByUrl('login');
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

  setCookie(name: string, value: string, days: number) {
    const expires = new Date();
    expires.setTime(expires.getTime() + days * 24 * 60 * 60 * 1000);
    const expiresString = 'expires=' + expires.toUTCString();
    document.cookie = `${name}=${value}; ${expiresString}; path=/`;
  }
}
