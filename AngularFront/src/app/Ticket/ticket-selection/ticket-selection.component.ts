import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/data.service';
import { Router } from '@angular/router'; // 用於跳轉到下一個畫面

@Component({
  selector: 'app-ticket-selection',
  templateUrl: './ticket-selection.component.html',
  styleUrls: ['./ticket-selection.component.css'],
})
export class TicketSelectionComponent implements OnInit {
  ticketOptions = [0, 1, 2, 3, 4, 5]; // 可以選擇的票數
  selectedTicket = {
    fullVote: 0,
    studentVote: 0,
    oldpeopleTicket: 0,
  };
  totalPrice: number = 0;

  // 新增的變量來保存電影資訊
  movieName: string = '';
  cinemaName: string = '';
  hallName: string = '';
  showtime: string = '';
  posterUrl: string = '';
  releaseDate: string = '';

  constructor(
    private route: ActivatedRoute,
    private dataService: DataService,
    private router: Router // 用於跳轉
  ) {}

  ngOnInit(): void {
    // 使用 ActivatedRoute 來接收 URL 參數
    this.route.queryParams.subscribe((params) => {
      this.movieName = params['movieName'] || '';
      this.cinemaName = params['cinemaName'] || '';
      this.hallName = params['hallName'] || '';
      this.showtime = params['showtime'] || '';
      this.posterUrl = params['posterUrl'] || '';
      this.releaseDate = params['releaseDate'] || '';
    });

    this.calculateTotal(); // 初始化時計算總價
  }

  calculateTotal() {
    this.totalPrice =
      this.selectedTicket.fullVote * 200 +
      this.selectedTicket.studentVote * 180 +
      this.selectedTicket.oldpeopleTicket * 100;
  }

  // 新增提交訂單的邏輯
  submitReservation() {
    const reservationData = {
      memberID: 1, // 假設目前使用者是 1，之後可從會員系統獲取
      showtimeID: 2, // 假設該場次的 ID 已知
      totalPrice: this.totalPrice,
      ticketCount:
        this.selectedTicket.fullVote +
        this.selectedTicket.studentVote +
        this.selectedTicket.oldpeopleTicket,
      paymentMethod: '信用卡', // 您可以更改支付方式
      couponID: null, // 如果有折扣券可以填入
    };

    // 調用 API 生成訂單
    this.dataService.createReservation(reservationData).subscribe(
      (response) => {
        // 成功後跳轉到下一個畫面
        console.log('訂單成功生成:', response);
        this.router.navigate(['ticket/ticketreservation']); // 跳轉到訂單頁面
      },
      (error) => {
        console.error('生成訂單失敗:', error);
      }
    );
  }
}
