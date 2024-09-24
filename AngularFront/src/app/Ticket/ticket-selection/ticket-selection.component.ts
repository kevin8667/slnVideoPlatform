import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/data.service';
import { Router } from '@angular/router'; // 用於跳轉到下一個畫面
import { Location } from '@angular/common'; // 新增 Location 模組來實現返回功能
import { memberName } from 'src/app/interfaces/forumInterface/memberIName';

@Component({
  selector: 'app-ticket-selection',
  templateUrl: './ticket-selection.component.html',
  styleUrls: ['./ticket-selection.component.css'],
})
export class TicketSelectionComponent implements OnInit {
  user: memberName = {
    memberId: 0,
    nickName: '',
  };


  ticketOptions = [0, 1, 2, 3, 4, 5]; // 可以選擇的票數
  selectedTicket = {
    fullVote: 0,
    studentVote: 0,
    oldpeopleTicket: 0,
  };
  totalPrice: number = 0;
  TicketCount: number = 0;

  // 新增的變量來保存電影資訊
  movieName: string = '';
  movieId: number = 0;
  cinemaName: string = '';
  hallName: string = '';
  date: string = '';
  day: string = '';
  showtime: string = '';
  showtimeId: number = 0; // 儲存放映的場次 ID
  posterUrl: string = '';
  summary: string = '';
  runningTime: string = '';

  constructor(
    private route: ActivatedRoute,
    private dataService: DataService,
    private router: Router, // 用於跳轉
    private location: Location // 注入 Location 服務
  ) {}

  // 返回上頁的邏輯
  goBack() {
    this.location.back(); // 調用 location.back() 返回上個頁面
  }
  ngOnInit(): void {

    this.dataService.user$.subscribe((data) => (this.user = data));
    // 從 localStorage 中讀取資料
    this.movieName = localStorage.getItem('movieName') || '';
    this.movieId = parseInt(localStorage.getItem('movieId') || '0', 10);
    this.cinemaName = localStorage.getItem('cinemaName') || '';
    this.hallName = localStorage.getItem('hallName') || '';
    this.date = localStorage.getItem('date') || '';
    this.day = localStorage.getItem('day') || '';
    this.showtime = localStorage.getItem('showtime') || '';
    this.showtimeId = parseInt(
      localStorage.getItem('selectedShowtimeId') || '0',
      10
    );
    this.posterUrl = localStorage.getItem('posterUrl') || '';
    this.summary = localStorage.getItem('summary') || '';
    this.runningTime = localStorage.getItem('runningTime') || '';

    console.log('從 Local Storage 中讀取的資料:', {
      movieName: this.movieName,
      movieId: this.movieId,
      cinemaName: this.cinemaName,
      hallName: this.hallName,
      date: this.date,
      day: this.day,
      showtime: this.showtime,
      showtimeId: this.showtimeId,
      posterUrl: this.posterUrl,
      runningTime: this.runningTime,
    });

    this.calculateTotal(); // 初始化時計算總價
  }

  calculateTotal() {
    this.totalPrice =
      this.selectedTicket.fullVote * 200 +
      this.selectedTicket.studentVote * 180 +
      this.selectedTicket.oldpeopleTicket * 100;
    this.TicketCount =
      this.selectedTicket.fullVote * 1 +
      this.selectedTicket.studentVote * 1 +
      this.selectedTicket.oldpeopleTicket * 1;

    // 输出计算后的结果到控制台
    console.log('Total Price:', this.totalPrice);
    console.log('Ticket Count:', this.TicketCount);
  }
  submitReservation() {
    // 檢查是否至少選擇了一張票
    if (
      this.selectedTicket.fullVote === 0 &&
      this.selectedTicket.studentVote === 0 &&
      this.selectedTicket.oldpeopleTicket === 0
    ) {
      window.alert('請選擇至少一張票');
      return; // 如果未選擇票，終止後續提交操作
    }

    const reservationData = {
      memberID: this.user.memberId, // 使用者 ID，
      showtimeID: this.showtimeId,
      totalPrice: this.totalPrice,
      ticketCount: this.TicketCount,
      paymentMethod: '信用卡', // 您可以更改支付方式
      couponID: null, // 如果有折扣券可以填入
    };

    console.log('傳入參數：' + reservationData);

    // 調用 API 生成訂單
    this.dataService.createReservation(reservationData).subscribe(
      (reservationResponse) => {
        // 成功後將資料儲存到 Local Storage
        localStorage.setItem('reservationId', reservationResponse.toString());
        localStorage.setItem('movieId', this.movieId.toString());
        localStorage.setItem('movieName', this.movieName);
        localStorage.setItem('hallName', this.hallName);
        localStorage.setItem('showtimeId', this.showtimeId.toString());
        localStorage.setItem('date', this.date);
        localStorage.setItem('day', this.day);
        localStorage.setItem('showtime', this.showtime);
        localStorage.setItem('runningTime', this.runningTime);

        localStorage.setItem('totalPrice', this.totalPrice.toString());
        localStorage.setItem('ticketCount', this.TicketCount.toString());
        //票
        localStorage.setItem(
          'fullVote',
          this.selectedTicket.fullVote.toString()
        );
        localStorage.setItem(
          'studentVote',
          this.selectedTicket.studentVote.toString()
        );
        localStorage.setItem(
          'oldpeopleTicket',
          this.selectedTicket.oldpeopleTicket.toString()
        );

        // 跳轉到下一個畫面，這裡不再使用 queryParams
        this.router.navigate(['ticket/ticketreservation']);
      },
      (error) => {
        console.error('生成訂單失敗:', error);
      }
    );
  }
}
