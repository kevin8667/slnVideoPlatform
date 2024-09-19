import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-error-page',
  templateUrl: './error-page.component.html',
  styleUrls: ['./error-page.component.css']
})
export class ErrorPageComponent implements OnInit{
  constructor(private router: Router) { }

  ngOnInit(): void {
    // 三秒後自動導航到首頁
    setTimeout(() => {
      this.router.navigate(['/']);
    }, 3000);  // 3000 毫秒 = 3 秒
  }
}
