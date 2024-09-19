import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from 'src/app/data.service';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.css'],
  providers: [DataService],
})
export class TicketComponent implements OnInit {
  movieId: number = 4; // 測試用電影ID
  selectedCinemaId: number | null = null; // 選中的影院ID
  selectedCinema: any = null; // 選中的影院
  cinemas: any[] = []; // 影院清單
  showTimeID: number = 0;

  constructor(
    private router: Router,
    private dataService: DataService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    if (this.movieId) {
      this.dataService.getCinemas(this.movieId).subscribe((data: any) => {
        this.cinemas = data.map((cinema: any) => ({
          CinemaId: cinema.cinemaId,
          CinemaName: cinema.cinemaName,
          Halls: [],
        }));

        const savedCinemaId = localStorage.getItem('selectedCinemaId');

        if (savedCinemaId) {
          this.selectedCinemaId = parseInt(savedCinemaId, 10);
          this.selectedCinema = this.cinemas.find(
            (cinema) => cinema.CinemaId === this.selectedCinemaId
          );
        }

        // 如果有預設選中的影院，則載入該影院的放映時間
        if (this.selectedCinema) {
          this.loadShowtimes(this.selectedCinema.CinemaId);
        }

        // 手動觸發變更檢測
        this.cdr.detectChanges();
      });
    } else {
      console.error('沒有電影ID');
    }

  }

  onCinemaChange(event: any) {
    this.selectedCinemaId = parseInt(event.target.value, 10);
    this.selectedCinema = this.cinemas.find(
      (cinema) => cinema.CinemaId === this.selectedCinemaId
    );

    // 將選中的影院ID儲存到 localStorage
    localStorage.setItem('selectedCinemaId', this.selectedCinemaId.toString());

    // 載入放映時間
    this.loadShowtimes(this.selectedCinema.CinemaId);

    // 手動觸發變更檢測
    this.cdr.detectChanges();
  }

  loadShowtimes(cinemaId: number) {
    this.dataService.getShowtimesByCinema(cinemaId).subscribe((data: any) => {
      const halls = data.reduce((acc: any[], showtime: any) => {
        let hall = acc.find((h) => h.HallName === showtime.hallsName);
        if (!hall) {
          hall = { HallName: showtime.hallsName, Showtimes: [] };
          acc.push(hall);
        }

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

        hall.Showtimes.push({
          time,
          date,
          day,
          showTimeID: showtime.showtimeId,
          showTimeDate // 我們要保存原始日期進行排序
        });

        return acc;
      }, []);

      // 對每個影廳的 Showtimes 進行日期和時間排序
      halls.forEach((hall: { Showtimes: any[] }) => {
        hall.Showtimes.sort((a: any, b: any) => {
          // 首先比較日期，然後比較時間
          const dateComparison = new Date(a.showTimeDate).getTime() - new Date(b.showTimeDate).getTime();
          if (dateComparison !== 0) {
            return dateComparison; // 如果日期不同，按照日期排序
          }
          // 如果日期相同，則按照時間排序
          return a.time.localeCompare(b.time);
        });
      });

      // 更新選中的影院的 Halls
      this.selectedCinema.Halls = halls;

      // 手動觸發變更檢測
      this.cdr.detectChanges();
    });
  }
  // 當點擊選擇時間按鈕時，將選中的資料儲存到 Local Storage，並跳轉至下一個頁面
  onTimeSelect(hallName: string, time: string, showTimeID: number) {
    console.log('選中的 showtimeId:', showTimeID);

    // 儲存選中的資料到 localStorage
    localStorage.setItem('cinemaName', this.selectedCinema.CinemaName);
    localStorage.setItem('hallName', hallName);
    localStorage.setItem('showtime', time);
    localStorage.setItem('selectedShowtimeId', showTimeID.toString());
    localStorage.setItem('movieId', this.movieId.toString());
    localStorage.setItem('movieName', 'Deadpool & Wolverine');
    localStorage.setItem('posterUrl', '../../../../assets/image/Deadpool_Wolverine.png');
    localStorage.setItem('releaseDate', '2024/07/24');

    // 跳轉到下一個頁面
    this.router.navigate(['ticket/ticketselection']);
  }
  // // 當點擊選擇時間按鈕時，傳遞選中的上映時間至下一頁面
  // onTimeSelect(hallName: string, time: string, showTimeID: number) {
  //   console.log('選中的 showtimeId:', showTimeID);

  //   // 傳遞選中的上映時間至第二個畫面
  //   this.router.navigate(['ticket/ticketselection'], {
  //     queryParams: {
  //       cinemaName: this.selectedCinema.CinemaName,
  //       hallName: hallName,
  //       showtime: time, // 傳遞選中的上映時間
  //       selectedShowtimeId: showTimeID,
  //       movieId: this.movieId,
  //       movieName: 'Deadpool & Wolverine',
  //       posterUrl: '../../../../assets/image/Deadpool_Wolverine.png',
  //       releaseDate: '2024/07/24',
  //     },
  //   });
  // }

  // goToSelection() {
  //   window.location.href = '../../../ticketselection'; // 進入選擇票券頁面
  // }
}
