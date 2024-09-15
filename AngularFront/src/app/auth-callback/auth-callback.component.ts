import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-auth-callback',
  template: `<p>Processing...</p>`,
  styles: []
})
export class AuthCallbackComponent implements OnInit {

  constructor(private route: ActivatedRoute, private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    debugger;
    // Extract authorization code from query parameters
    this.route.queryParams.subscribe(params => {
      debugger;
      const fullUrl = window.location.href;

      // Extract the code parameter from the URL
      const code = this.extractCodeFromUrl(fullUrl);
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
  
  private extractCodeFromUrl(url: string): string | null {
    try {
      // Create a URL object
      const urlObj = new URL(url);
      
      // Use URLSearchParams to get the code parameter
      const searchParams = new URLSearchParams(urlObj.search);

      // Return the value of the 'code' parameter
      return searchParams.get('code');
    } catch (error) {
      console.error('Error extracting code from URL', error);
      return null;
    }
  }

  // Exchange authorization code for an access token
  private exchangeCodeForToken(code: string): void {
    debugger;
    this.http.post('https://localhost:7193/api/auth/callback', { code })
      .subscribe(response => {
        // Handle the response and store the token if needed
        debugger;
        console.log('Token received', response);

        // Redirect to the home page or dashboard after successful login
      //  this.router.navigate(['/']);
      }, error => {
        console.error('Error exchanging code for token', error);
       // this.router.navigate(['./login']); // Redirect to login page or error page
      });
  }
}