import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-finish-payment',
  templateUrl: './finish-payment.component.html',
  styleUrls: ['./finish-payment.component.css']
})
export class FinishPaymentComponent {

  countdown:number = 5;

  constructor(private router: Router) { }

  ngOnInit(): void {
    // // 設置5秒延遲後跳轉到首頁
    // setTimeout(() => {
    //   this.router.navigate(['/']); // 導航到首頁
    // }, 5000); // 5秒 = 5000 毫秒
    // 設置倒數計時器，每秒減少1
    const interval = setInterval(() => {
      this.countdown--;
      if (this.countdown === 0) {
        clearInterval(interval); // 清除計時器
        this.router.navigate(['/']); // 導航到首頁
      }
    }, 1000); // 每秒執行一次
  }
}
