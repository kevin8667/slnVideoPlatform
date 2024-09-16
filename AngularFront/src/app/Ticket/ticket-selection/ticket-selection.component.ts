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
  TicketCount: number = 0;

  // 新增的變量來保存電影資訊
  movieName: string = '';
  movieId: number = 0;
  cinemaName: string = '';
  hallName: string = '';
  showtime: string = '';
  showtimeId: number = 0; // 儲存放映的場次 ID
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
      this.movieId = +params['movieId'] || 0;
      this.cinemaName = params['cinemaName'] || '';
      this.hallName = params['hallName'] || '';
      this.showtime = params['showtime'] || '';
      this.showtimeId = +params['selectedShowtimeId'] || 0; // 轉換為數字類型的 showtimeId
      this.posterUrl = params['posterUrl'] || '';
      this.releaseDate = params['releaseDate'] || '';
    });

    console.log('接收到的參數:', {
      movieName: this.movieName,
      movieId: this.movieId,
      cinemaName: this.cinemaName,
      hallName: this.hallName,
      showtime: this.showtime,
      showtimeId: this.showtimeId,
      posterUrl: this.posterUrl,
      releaseDate: this.releaseDate,
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
    const reservationData = {
      memberID: 1, // 使用者 ID，假設目前使用者是 1
      showtimeID: this.showtimeId,
      totalPrice: this.totalPrice,
      ticketCount: this.TicketCount,
      paymentMethod: '信用卡', // 您可以更改支付方式
      couponID: null, // 如果有折扣券可以填入
    };

    // 調用 API 生成訂單
    this.dataService.createReservation(reservationData).subscribe(
      (reservationResponse) => {
        console.log('訂單成功生成:', reservationResponse);

        // 緊接著生成座位
        const seatAssignmentRequest = {
          ReservationId: reservationResponse, // 從生成的訂單中獲取 reservationId
          ShowtimeId: this.showtimeId,
          TicketCount: this.TicketCount,
        };
        console.log(seatAssignmentRequest.ReservationId);

        // 調用生成座位的 API
        this.assignSeats(seatAssignmentRequest); // 呼叫 assignSeats() 函數
      },
      (error) => {
        console.error('生成訂單失敗:', error);
      }
    );
  }

  // 調用座位生成 API
  assignSeats(seatAssignmentRequest: {
    ReservationId: number;
    ShowtimeId: number;
    TicketCount: number;
  }) {
    console.log('生成座位請求數據:', seatAssignmentRequest); // 這裡檢查要傳給 API 的請求數據

    this.dataService.assignSeats(seatAssignmentRequest).subscribe(
      (seatResponse) => {
        console.log('座位成功生成:', seatResponse);

        // 跳轉到下一個畫面，並傳遞座位資訊和訂單資訊
        this.router.navigate(['ticket/ticketreservation'], {
          queryParams: {
            reservationId: seatAssignmentRequest.ReservationId,
            assignedSeats: seatResponse.assignedSeats, // 傳遞生成的座位
            cinemaName: this.cinemaName,
            hallName: this.hallName,
            showtime: this.showtime,
            showtimeId: this.showtimeId,
            // movieId: this.movieId,
            movieName: this.movieName,
            posterUrl: this.posterUrl,
            releaseDate: this.releaseDate,
          },
        });
      },
      (seatError) => {
        console.error('生成座位失敗:', seatError);
      }
    );
  }
  // 新增提交訂單的邏輯
  // submitReservation() {
  //   const reservationData = {
  //     memberID: 1, // 假設目前使用者是 1，之後可從會員系統獲取
  //     showtimeID: this.showtimeId,
  //     totalPrice: this.totalPrice,
  //     ticketCount:
  //       this.selectedTicket.fullVote +
  //       this.selectedTicket.studentVote +
  //       this.selectedTicket.oldpeopleTicket,
  //     paymentMethod: '信用卡', // 您可以更改支付方式
  //     couponID: null, // 如果有折扣券可以填入
  //   };

  //   // 調用 API 生成訂單
  //   this.dataService.createReservation(reservationData).subscribe(
  //     (response) => {
  //       // 成功後跳轉到下一個畫面
  //       console.log('訂單成功生成:', response);
  //       this.router.navigate(['ticket/ticketreservation']); // 跳轉到訂單頁面
  //     },
  //     (error) => {
  //       console.error('生成訂單失敗:', error);
  //     }
  //   );
  // }
}
