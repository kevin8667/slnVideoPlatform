import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/data.service';
import { ActivatedRoute } from '@angular/router'; // 用於接收 reservationID
import { Location } from '@angular/common'; // 用於返回上頁
import { Router } from '@angular/router';

@Component({
  selector: 'app-ticket-reservation',
  templateUrl: './ticket-reservation.component.html',
  styleUrls: ['./ticket-reservation.component.css'],
})
export class TicketReservationComponent implements OnInit {
  reservationID: number = 0; // 儲存訂單 ID
  movieId: number = 0; // 儲存電影ID
  movieName: string = ''; // 儲存電影名稱
  hallName: string = ''; // 儲存影廳名稱
  showtime: string = ''; // 儲存放映時間
  showtimeId: number = 0; // 儲存放映場次ID
  totalPrice: number = 0; // 儲存總價
  ticketCount: number = 0; // 儲存票數
  seats: string[] = []; // 儲存座位列表
  fullVote: number = 0;
  studentVote: number = 0;
  oldpeopleTicket: number = 0;

  constructor(
    private dataService: DataService,
    private location: Location, // 注入 Location 服務
    private router: Router, // 注入 Router 服務來進行導航
    // private route: ActivatedRoute,

  ) {}

  ngOnInit(): void {
    // 從 Local Storage 中讀取資料
    this.reservationID = parseInt(
      localStorage.getItem('reservationId') || '0',
      10
    );
    this.movieId = parseInt(localStorage.getItem('movieId') || '0', 10);
    this.movieName = localStorage.getItem('movieName') || '';
    this.hallName = localStorage.getItem('hallName') || '';
    this.showtimeId = parseInt(localStorage.getItem('showtimeId') || '0', 10);
    this.showtime = localStorage.getItem('showtime') || '';
    this.totalPrice = parseInt(localStorage.getItem('totalPrice') || '0', 10);
    this.ticketCount = parseInt(localStorage.getItem('ticketCount') || '0', 10);
    this.fullVote = parseInt(localStorage.getItem('fullVote') || '0', 10);
    this.studentVote = parseInt(localStorage.getItem('studentVote') || '0', 10);
    this.oldpeopleTicket = parseInt(
      localStorage.getItem('oldpeopleTicket') || '0',
      10
    );

    // 調用方法載入訂單詳細資料或生成座位號
    this.generateSeats();
  }

  // 根據票數生成座位號
  generateSeats() {
    const rowNumber = Math.floor(Math.random() * 5) + 1; // 假設為第2排
    let startingSeat = Math.floor(Math.random() * 15) + 1; // 隨機生成1到15的座位號

    // 清空之前的座位資料，以避免重新生成座位後重複顯示
    this.seats = [];

    for (let i = 0; i < this.ticketCount; i++) {
      this.seats.push(`${rowNumber}排${startingSeat + i}號`);
    }

    console.log('生成的座位:', this.seats); // 檢查生成的座位
  }

  // 跳轉到付款頁面的邏輯
  goToPayment() {
    // 可以在這裡存儲需要傳遞到付款頁面的資訊到 Local Storage 或 Session Storage
    this.router.navigate(['shoppingCart','finish']); // 跳轉到 FinishPaymentComponent
  }

  // 新增 goBack() 方法，用於返回上一頁
  goBack() {
    this.location.back(); // 使用 location.back() 返回上一頁

    //this.currentStep = this.step[0];
  }
}
