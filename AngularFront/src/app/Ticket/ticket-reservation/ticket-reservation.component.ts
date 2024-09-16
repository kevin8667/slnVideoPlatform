import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/data.service';
import { ActivatedRoute } from '@angular/router'; // 用於接收 reservationID

@Component({
  selector: 'app-ticket-reservation',
  templateUrl: './ticket-reservation.component.html',
  styleUrls: ['./ticket-reservation.component.css'],
})
export class TicketReservationComponent implements OnInit {
  reservationID: number = 0; // 儲存訂單 ID
  movieName: string = '';
  hallName: string = '';
  showtime: string = '';
  ticketDetails: any; // 儲存訂單詳細資料
  seats: string[] = []; // 儲存座位列表

  constructor(private dataService: DataService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    // 1. 獲取訂單 ID 從 URL
    this.route.queryParams.subscribe((params) => {
      this.reservationID = +params['reservationID']; // 確保轉成數字
      this.loadReservationDetails(); // 載入訂單詳情
    });
  }

  // 2. 調用 API 生成座位並取得訂單詳情
  loadReservationDetails() {
    const seatSelectionData = {
      reservationId: this.reservationID,
      showtimeId: 2, // 假設該場次的 ID
      hallId: 1, // 假設影廳 ID
      ticketCount: 3, // 假設票數
    };

    this.dataService.assignSeats(seatSelectionData).subscribe(
      (response) => {
        // 解析 API 回應
        this.movieName = 'Deadpool & Wolverine'; // 您可以從後端獲取這些資料
        this.hallName = '秀泰板橋1廳'; // 假設影廳名稱
        this.showtime = '2024/08/06 11:30'; // 假設放映時間
        this.seats = response.SelectedSeats; // 儲存分配的座位
        this.ticketDetails = response; // 假設完整訂單資訊在這裡

      },
      (error) => {
        console.error('生成座位失敗:', error);
      }
    );
  }
}


// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-ticket-reservation',
//   templateUrl: './ticket-reservation.component.html',
//   styleUrls: ['./ticket-reservation.component.css'],
// })
// export class TicketReservationComponent {
//   ticketOptions = [0, 1, 2, 3, 4, 5]; // 可以選擇的票數
//   selectedTicket = {
//     earlyBird: 0,
//     cardDiscount: 0,
//     loveTicket: 0,
//   };
//   totalPrice: number = 0;

//   ngOnInit(): void {
//     this.calculateTotal(); // 初始化時計算總價
//   }

//   calculateTotal() {
//     this.totalPrice =
//       this.selectedTicket.earlyBird * 200 +
//       this.selectedTicket.cardDiscount * 180 +
//       this.selectedTicket.loveTicket * 100;
//   }
// }
