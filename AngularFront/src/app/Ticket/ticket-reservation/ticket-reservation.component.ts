import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/data.service';
import { ActivatedRoute } from '@angular/router'; // 用於接收 reservationID
import { Location } from '@angular/common'; // 用於返回上頁

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
    private route: ActivatedRoute,
    private location: Location // 注入 Location 服務
  ) {}

  ngOnInit(): void {
    // 1. 獲取從上一個畫面傳來的 queryParams
    this.route.queryParams.subscribe((params) => {
      this.reservationID = +params['reservationId']; // 轉為數字
      this.movieId = +params['movieId']; // 轉為數字
      this.movieName = params['movieName'];
      this.hallName = params['hallName'];
      this.showtimeId = +params['showtimeId']; // 轉為數字
      this.showtime = params['showtime'];
      this.totalPrice = +params['totalPrice']; // 轉為數字
      this.ticketCount = +params['ticketCount']; // 轉為數字
      this.fullVote = +params['fullVote'];
      this.studentVote = +params['studentVote'];
      this.oldpeopleTicket = +params['oldpeopleTicket'];

      // 調用方法載入訂單詳細資料
      // this.loadReservationDetails();

      // 根據票數生成座位號
      this.generateSeats();
    });
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
  // 新增 goBack() 方法，用於返回上一頁
  goBack() {
    this.location.back(); // 使用 location.back() 返回上一頁
  }
}
