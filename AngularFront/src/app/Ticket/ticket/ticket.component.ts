import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from 'src/app/data.service';
// import { TicketSelectionComponent } from '../ticket-selection/ticket-selection.component';
@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.css'],
  providers: [DataService],
})
export class TicketComponent implements OnInit {
  movieId: number = 4; // 測試用電影ID，您可以更換此ID為實際接收到的ID

  selectedCinema: any = null; // 選中的影院
  cinemas: any[] = []; // 影院清單
  showTime: any;
  showtime: any;

  constructor(private router: Router, private dataService: DataService) {}

  ngOnInit(): void {
    // 2. 在 ngOnInit 中使用 movieId 呼叫 API
    if (this.movieId) {
      // 3. 調用 DataService 的 getCinemasByMovie 方法，傳入 movieId
      this.dataService.getCinemas(this.movieId).subscribe((data: any) => {
        this.cinemas = data.map((cinema: any) => ({
          CinemaId: cinema.cinemaId, // 確保保存 cinemaId
          CinemaName: cinema.cinemaName,
          Halls: [], // 暫時將 Halls 設置為空陣列，稍後填充
        }));

        // 預設選中第一個影院並加載放映時間
        if (this.cinemas.length > 0) {
          this.selectedCinema = this.cinemas[0];
          this.loadShowtimes(this.selectedCinema.CinemaId); // 請求時傳 cinemaId
        }
      });
    } else {
      // 如果沒有電影ID，您可以處理錯誤或提示用戶
      console.error('沒有電影ID');
    }
  }

  // 當選擇影院時觸發此方法
  onCinemaChange(event: any) {
    const selectedCinemaId = event.target.value; // 這裡需要 cinemaId 而非 cinemaName
    this.selectedCinema = this.cinemas.find(
      (cinema) => cinema.CinemaId === parseInt(selectedCinemaId, 10) // 找到選中的影院
    );
    this.loadShowtimes(this.selectedCinema.CinemaId); // 傳送 cinemaId 給 API
  }

  // 根據選中的影院載入影廳與放映時間
  loadShowtimes(cinemaId: number) {
    // 根據 API 獲取影院的放映時間
    this.dataService.getShowtimesByCinema(cinemaId).subscribe((data: any) => {
      const halls = data.reduce((acc: any[], showtime: any) => {
        let hall = acc.find((h) => h.HallName === showtime.hallsName);
        if (!hall) {
          hall = { HallName: showtime.hallsName, Showtimes: [] };
          acc.push(hall);
        }

        // 24 小時制時間並放入 Showtimes 陣列
        // hall.Showtimes.push(
        //   new Date(showtime.showTimeDatetime).toLocaleTimeString([], {
        //     hour: '2-digit',
        //     minute: '2-digit',
        //     hour12: false,
        //   })
        // );
        // 24 小時制時間，日期與星期加入 Showtimes 陣列
        const showTimeDate = new Date(showtime.showTimeDatetime);
        const time = showTimeDate.toLocaleTimeString([], {
          hour: '2-digit',
          minute: '2-digit',
          hour12: false,
        });
        const date = showTimeDate.toLocaleDateString([], {
          month: 'numeric',
          day: 'numeric',
        });
        const day = showTimeDate.toLocaleDateString([], { weekday: 'long' });

        hall.Showtimes.push({ time, date, day });
        return acc;
      }, []);

      // 對每個影廳的 Showtimes 進行排序
      // halls.forEach((hall: { Showtimes: string[] }) => {
      //   hall.Showtimes.sort((a: string, b: string) => {
      //     return a.localeCompare(b); // 按時間排序
      //   });
      // });

      // 對每個影廳的 Showtimes 進行排序
      halls.forEach((hall: { Showtimes: any[] }) => {
        hall.Showtimes.sort((a: any, b: any) => {
          return a.time.localeCompare(b.time); // 按時間排序
        });
      });

      // 對 halls 進行排序，按照廳的名稱中的數字排序
      halls.sort(
        (
          a: { HallName: { match: (arg0: RegExp) => string[] } },
          b: { HallName: { match: (arg0: RegExp) => string[] } }
        ) => {
          const hallNumberA = parseInt(a.HallName.match(/\d+/)[0], 10);
          const hallNumberB = parseInt(b.HallName.match(/\d+/)[0], 10);
          return hallNumberA - hallNumberB;
        }
      );
      // 將取得的影廳和放映時間放入 selectedCinema 的 Halls 中
      this.selectedCinema.Halls = halls;
    });
  }

  onTimeSelect(hallName: string, showtime: string) {
    // 跳轉到購票頁面，並傳遞相關資訊
    this.router.navigate(['ticket/ticketselection'], {
      queryParams: {
        cinemaName: this.selectedCinema.CinemaName,
        hallName: hallName,
        showtime: showtime,
        movieId: this.movieId,
        movieName: 'Deadpool & Wolverine',
        posterUrl: '../../../../assets/image/Deadpool_Wolverine.png',
        releaseDate: '2024/07/24',
      },
    });
  }

  goToSelection() {
    window.location.href = '../../../ticketselection'; // 進入選擇票券頁面
  }

  // 最基本
  //constructor(private router: Router) {}
  // ngOnInit(): void {
  //   throw new Error('Method not implemented.');
  // }
  // selectedCinema: any = null;
  // cinemas: any[] = [
  //   {
  //     CinemaName: '秀泰新北板橋',
  //     Halls: [
  //       {
  //         HallName: '影廳 1',
  //         Showtimes: ['14:35', '19:05'],
  //       },
  //       {
  //         HallName: '影廳 2',
  //         Showtimes: ['19:00', '21:08'],
  //       },
  //     ],
  //   },
  //   {
  //     CinemaName: '新光影城台中大遠百',
  //     Halls: [
  //       {
  //         HallName: '影廳 1',
  //         Showtimes: ['15:00', '20:00'],
  //       },
  //       {
  //         HallName: '影廳 2',
  //         Showtimes: ['16:00', '22:00'],
  //       },
  //     ],
  //   },
  // ];

  // onCinemaChange(event: any) {
  //   const selectedCinemaName = event.target.value;
  //   this.selectedCinema = this.cinemas.find(cinema => cinema.CinemaName === selectedCinemaName);
  // }

  // goToSelection() {
  //   window.location.href = '../../../ticket-selection';
  // }
}
